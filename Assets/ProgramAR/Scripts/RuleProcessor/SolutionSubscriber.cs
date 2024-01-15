 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.MoveitTaskConstructor;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Moveit;
using ProgramAR.Variables;
using RosMessageTypes.Geometry;
using RosMessageTypes.TaskPlanner;
using ProgramAR.Pages;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Sensor;
/// <summary>
///
/// </summary>

namespace RuleProcessor
{
    public class SolutionSubscriber : MonoBehaviour, ISimulateClauseResponse
    {
        public static readonly string[] LinkNames =
        { "world/fetch/base_link/torso_lift_link", "/shoulder_pan_link", "/shoulder_lift_link", "/upperarm_roll_link", "/elbow_flex_link", "/forearm_roll_link", "/wrist_flex_link", "wrist_roll_link" };

        Stage currentStage = Stage.IDLE;
        enum Stage { IDLE, GET_FAKE_PLAN, FAKE_SIMULATION, REAL_SIMULATION };

        [SerializeField] GameObject m_RightGripper;
        [SerializeField] GameObject m_LeftGripper;

        [SerializeField]
        JointController[] m_OrderedJoints;
        [SerializeField] BoolVariable debug; 
        // Robot Joints
        [SerializeField] List<ArticulationBody> m_JointArticulationBodies;
        const float k_JointAssignmentWait = 0.07f;
        const float k_JointAssignmentWaithands = 0.0005f;
        const float k_PoseAssignmentWait = 0.3f;
        public GameObject m_Fetch;

        ROSConnection m_Ros;

        public string planSolutionTopicName = "/task_planner/pick_and_place/solution";
        public string executeSolutionResultTopicName = "/execute_task_solution/result";
        public string jointStateTopicName = "/joint_states";
        public SolutionScriptableObject sol;
        [SerializeField] UpdatePlanningSceneEvent updatePlanningSceneEvent;
        [SerializeField] DeselectClauseSimButtonEvent deselectClauseSimButtonEvent;
        [SerializeField] ClauseRuntimeSet actionClauseRuntimeSet;
        [SerializeField] ActionStatusVariable actionStatus;
        [SerializeField] ActionProcessor actionProcessor;
        List<(string, ZoneObject)> actionList = new List<(string, ZoneObject)>();

        bool fakeSimulating = false;
        int numCommands = 0;
        int currCommand = 0;
        bool realSimulating = false;
        bool planRecieved = false; 
        bool jointsAreSet = false;
        bool fingersAreSet = false;
        private SolutionMsg showRealAction = new SolutionMsg();
        [SerializeField] IntegerVariable currentAction;

        PickPlaceMsg actionMsg = new PickPlaceMsg();

        [SerializeField] UpdateRunStatusEvent updateRunStatusEvent; 

        bool waitingForSolution = false;
        public string topicName = "/unity/query_planner";

        [SerializeField] SimulateClauseEvent simulateClauseEvent;
        [SerializeField] PlanStatusEvent planStatusEvent; 
        [SerializeField] List<CheckPointInsideBox> bounds;
        int selectedClause = -1; 
        void Start()
        {
            // Get ROS connection static instance
            m_Ros = ROSConnection.GetOrCreateInstance();
            m_Ros.Subscribe<SolutionMsg>(planSolutionTopicName, SolutionCallback);
            m_Ros.Subscribe<ExecuteTaskSolutionActionResult>(executeSolutionResultTopicName, SolutionResultCallback);
            m_Ros.Subscribe<JointStateMsg>(jointStateTopicName, JointStateCallback);
        }

        public void OnEnable()
        {
            simulateClauseEvent.RegisterListener(this);
        }

        public void OnDisable()
        {
            simulateClauseEvent.UnregisterListener(this);
        }

        private void Update()
        {
            switch (currentStage)
            {
                case Stage.IDLE:
                    break;
                case Stage.GET_FAKE_PLAN:
                    Log("Stage GET_FAKE_PLAN");
                    // Once every command has been planned, simulate each solution
                    if (numCommands == currCommand)
                    {
                        currCommand = 0;
                        currentStage = Stage.FAKE_SIMULATION;
                    }
                    else if (!waitingForSolution)
                    {
                        StartCoroutine(GetSimulationSolution(currCommand));
                        waitingForSolution = true;
                    }
                    break;
                case Stage.FAKE_SIMULATION:
                    Log("Stage FAKE_SIMULATION");
                    if (!fakeSimulating)
                    {
                        fakeSimulating = true;
                        TrajectoryResponse(sol.solution[currCommand]);
                    }
                    break;
                case Stage.REAL_SIMULATION:
                    //Log("Stage REAL_SIMULATION");
                    if (!realSimulating && planRecieved)
                    {
                        realSimulating = true;
                        TrajectoryResponse(showRealAction);
                    }
                    break;
            }
        }

        public void OnRunProgram()
        {
            if (currentStage == Stage.REAL_SIMULATION)
            {
                waitingForSolution = false;
                currentStage = Stage.IDLE;
            }
            else
            {
                Log("Running program");
                currentStage = Stage.REAL_SIMULATION;
                realSimulating = false;
                planRecieved = false;
                if (selectedClause != -1)
                {
                    deselectClauseSimButtonEvent.Raise(selectedClause, "Simulate Action");
                    selectedClause = -1;
                }
            }
        }

        public void OnSimulateClauseEvent(int selectedClause, bool simulate)
        {

            if (currentStage != Stage.REAL_SIMULATION)
            {
                planStatusEvent.Raise("Getting Simulation Plan");
                if (simulate == true)
                {
                    if (this.selectedClause != -1 && this.selectedClause != selectedClause)
                    {
                        deselectClauseSimButtonEvent.Raise(this.selectedClause, "Simulate Action");
                    }
                    currentStage = Stage.GET_FAKE_PLAN;
                    this.selectedClause = selectedClause; 
                    actionList = actionProcessor.EvaluateAction(actionClauseRuntimeSet.Items[selectedClause]);
                    if (actionList.Count == 0)
                    {
                        currentStage = Stage.IDLE;
                        waitingForSolution = false;
                        planStatusEvent.Raise("Action already complete");
                        deselectClauseSimButtonEvent.Raise(selectedClause, "Already Done");
                        this.selectedClause = -1; 
                    }
                    numCommands = actionList.Count;
                }
                else
                {
                    waitingForSolution = false; 
                    currentStage = Stage.IDLE;
                }
                currCommand = 0;
                fakeSimulating = false;
                sol.solution.Clear();
            }
        }

        public Vector3 ZonePlacementPosition(ZoneObject zone)
        {
            var outPos = zone.transform.localPosition;
            foreach (CheckPointInsideBox col in bounds)
            {
                if (col.IsInside(zone.transform.position))
                {
                    Log("Collider " + col.gameObject.name + " has point in bounds");
                    outPos.y = col.height;
                    break;
                }
            }

            return outPos;
        }

        public void JointStateCallback(JointStateMsg msg)
        {
            if (currentStage == Stage.IDLE && (!jointsAreSet || !fingersAreSet))
            {
                if (msg.position.Length == 2 && !fingersAreSet)
                {
                    m_LeftGripper.transform.localPosition = new Vector3((float)msg.position[0], 0f, 0f);
                    m_RightGripper.transform.localPosition = new Vector3(-(float)msg.position[1], 0f, 0f);
                    fingersAreSet = true; 
                }
                else if(msg.position.Length > 2)
                {
                   m_OrderedJoints[0].SetJointPosition((float)msg.position[2]);
                    m_OrderedJoints[1].SetJointPosition(-(float)msg.position[5]);
                    m_OrderedJoints[2].SetJointPosition((float)msg.position[6]);
                    m_OrderedJoints[3].SetJointPosition(-(float)msg.position[7]);
                    m_OrderedJoints[4].SetJointPosition((float)msg.position[8]);
                    m_OrderedJoints[5].SetJointPosition(-(float)msg.position[9]);
                    m_OrderedJoints[6].SetJointPosition((float)msg.position[10]);
                    m_OrderedJoints[7].SetJointPosition(-(float)msg.position[11]);
                    jointsAreSet = true;
                }
            }
            else if(currentStage != Stage.IDLE)
            {
                jointsAreSet = false;
                fingersAreSet = false; 
            }
        }

        void SolutionCallback(SolutionMsg solution)
        {
            switch (currentStage)
            {
                case Stage.REAL_SIMULATION:
                    planRecieved = true; 
                    showRealAction = solution;
                    break;
                case Stage.GET_FAKE_PLAN:
                    sol.solution.Add(solution);
                    currCommand++;
                    waitingForSolution = false;
                    break;
            }
        }

        IEnumerator GetSimulationSolution(int currentAction)
        {
            updatePlanningSceneEvent.Raise(actionList[currentAction].Item1);
            yield return new WaitForSeconds(3);

            var place = ZonePlacementPosition(actionList[currentAction].Item2).To<FLU>();

            actionMsg.task_type = PickPlaceMsg.PICK_AND_PLACE;
            actionMsg.grasp_frame = PickPlaceMsg.VERTICAL;
            actionMsg.execute = false;
            actionMsg.object_name = actionList[currentAction].Item1;
            actionMsg.place_pose = new PoseMsg(new PointMsg(place.x, place.y, place.z), new QuaternionMsg(0, 0, 0, 1f));

            m_Ros.Publish(topicName, actionMsg);


            Log("Published Simulated Command");
        }

        void TrajectoryResponse(SolutionMsg solution)
        {
            if (solution.sub_trajectory.Length > 0)
            {
                Log("Simulating Trajectory.");
                StartCoroutine(ExecuteTrajectories(solution));
            }
            else
            {
                if (currentStage == Stage.FAKE_SIMULATION)
                {
                    if (currCommand == numCommands)
                    {
                        currCommand = 0;
                    }
                }
                Debug.LogError("No trajectory returned from MoverService.");
            }
        }

        IEnumerator ExecuteTrajectories(SolutionMsg response)
        {
            if (response.sub_trajectory != null)
            {
                // For every sub trajectory
                foreach (SubTrajectoryMsg subTraj in response.sub_trajectory)
                {
                    if (subTraj.trajectory.joint_trajectory.joint_names != null)
                    {
                        // If it is not gripper open/close
                        if (subTraj.trajectory.joint_trajectory.joint_names.Length > 2)
                        {
                            // For every robot pose in sub trajectory plan
                            foreach (var t in subTraj.trajectory.joint_trajectory.points)
                            {
                                var jointPositions = t.positions;

                                // Set the joint values for every joint
                                for (var joint = 0; joint < m_OrderedJoints.Length; joint++)
                                {
                                    if (joint == 0 || joint == 2 || joint == 4 || joint == 6)
                                    {
                                        m_OrderedJoints[joint].SetJointPosition((float)jointPositions[joint]);
                                    }
                                    else
                                    {
                                        m_OrderedJoints[joint].SetJointPosition(-(float)jointPositions[joint]);
                                    }
                                }

                                // Wait for robot to achieve pose for all joint assignments
                                yield return new WaitForSeconds(k_JointAssignmentWait);
                            }
                        }
                        else
                        {
                            // For every robot pose in sub trajectory plan
                            foreach (var t in subTraj.trajectory.joint_trajectory.points)
                            {
                                var jointPositions = t.positions;

                                m_LeftGripper.transform.localPosition = new Vector3((float)jointPositions[0], 0f, 0f);
                                m_RightGripper.transform.localPosition = new Vector3(-(float)jointPositions[1], 0f, 0f);
                                // Wait for robot to achieve pose for all joint assignments
                                yield return new WaitForSeconds(k_JointAssignmentWaithands);
                            }
                        }

                        // Wait for the robot to achieve the final pose from joint assignment
                        yield return new WaitForSeconds(k_PoseAssignmentWait);
                    }
                }
            }
            realSimulating = false;
            fakeSimulating = false;

            currCommand++;
            if (currCommand == numCommands)
            {
                currCommand = 0;
            }
        }
        void SolutionResultCallback(ExecuteTaskSolutionActionResult result)
        {
            Log("Received callback " + result.result.error_code);
            currentAction.Value += 1;
            actionStatus.Done = true;
            actionStatus.ErrorCode = result.result.error_code;
            showRealAction = new SolutionMsg();
            waitingForSolution = false;
            planRecieved = false;
            
            if (result.result.error_code.val != MoveItErrorCodesMsg.SUCCESS)
            {
                if(currentStage == Stage.GET_FAKE_PLAN)
                {
                    planStatusEvent.Raise("Error: Zone too far or pick/place position too close to other boxes.");
                    deselectClauseSimButtonEvent.Raise(selectedClause, "Fix Scene");
                    currentStage = Stage.IDLE;
                }
                else if (currentStage == Stage.REAL_SIMULATION)
                {
                    planStatusEvent.Raise("Error: Zone too far or pick/place position too close to box.");
                    updateRunStatusEvent.Raise("Planning Failed");
                }

            }
            switch (result.result.error_code.val)
            {
                case MoveItErrorCodesMsg.SUCCESS:
                    planStatusEvent.Raise("Planning Success: Executing");
                    Log("SUCCESS");
                    break;
                case MoveItErrorCodesMsg.FAILURE:
                    LogWarning("FAILURE");
                    break;
                case MoveItErrorCodesMsg.PLANNING_FAILED:
                    LogWarning("PLANNING_FAILED");
                    break;
                case MoveItErrorCodesMsg.INVALID_MOTION_PLAN:
                    LogWarning("INVALID_MOTION_PLAN");
                    break;
                case MoveItErrorCodesMsg.MOTION_PLAN_INVALIDATED_BY_ENVIRONMENT_CHANGE:
                    LogWarning("FAILURE");
                    break;
                case MoveItErrorCodesMsg.CONTROL_FAILED:
                    LogWarning("CONTROL_FAILED");
                    break;
                case MoveItErrorCodesMsg.UNABLE_TO_AQUIRE_SENSOR_DATA:
                    LogWarning("UNABLE_TO_AQUIRE_SENSOR_DATA");
                    break;
                case MoveItErrorCodesMsg.TIMED_OUT:
                    LogWarning("TIMED_OUT");
                    break;
                case MoveItErrorCodesMsg.PREEMPTED:
                    LogWarning("PREEMPTED");
                    break;
                case MoveItErrorCodesMsg.START_STATE_IN_COLLISION:
                    LogWarning("START_STATE_IN_COLLISION");
                    break;
                case MoveItErrorCodesMsg.START_STATE_VIOLATES_PATH_CONSTRAINTS:
                    LogWarning("START_STATE_VIOLATES_PATH_CONSTRAINTS");
                    break;
                case MoveItErrorCodesMsg.GOAL_IN_COLLISION:
                    LogWarning("GOAL_IN_COLLISION");
                    break;
                case MoveItErrorCodesMsg.GOAL_VIOLATES_PATH_CONSTRAINTS:
                    LogWarning("GOAL_VIOLATES_PATH_CONSTRAINTS");
                    break;
                case MoveItErrorCodesMsg.GOAL_CONSTRAINTS_VIOLATED:
                    LogWarning("GOAL_CONSTRAINTS_VIOLATED");
                    break;
                case MoveItErrorCodesMsg.INVALID_GROUP_NAME:
                    LogWarning("INVALID_GROUP_NAME");
                    break;
                case MoveItErrorCodesMsg.INVALID_GOAL_CONSTRAINTS:
                    LogWarning("INVALID_GOAL_CONSTRAINTS");
                    break;
                case MoveItErrorCodesMsg.INVALID_ROBOT_STATE:
                    LogWarning("INVALID_ROBOT_STATE");
                    break;
                case MoveItErrorCodesMsg.INVALID_LINK_NAME:
                    LogWarning("INVALID_LINK_NAME");
                    break;
                case MoveItErrorCodesMsg.INVALID_OBJECT_NAME:
                    LogWarning("INVALID_OBJECT_NAME");
                    break;
                case MoveItErrorCodesMsg.FRAME_TRANSFORM_FAILURE:
                    LogWarning("FRAME_TRANSFORM_FAILURE");
                    break;
                case MoveItErrorCodesMsg.COLLISION_CHECKING_UNAVAILABLE:
                    LogWarning("COLLISION_CHECKING_UNAVAILABLE");
                    break;
                case MoveItErrorCodesMsg.ROBOT_STATE_STALE:
                    LogWarning("ROBOT_STATE_STALE");
                    break;
                case MoveItErrorCodesMsg.SENSOR_INFO_STALE:
                    LogWarning("SENSOR_INFO_STALE");
                    break;
                case MoveItErrorCodesMsg.COMMUNICATION_FAILURE:
                    LogWarning("COMMUNICATION_FAILURE");
                    break;
                case MoveItErrorCodesMsg.NO_IK_SOLUTION:
                    LogWarning("NO_IK_SOLUTION");
                    break;
                default:
                    LogWarning("Invalid Error Code");
                    break;
            }
        }
        private void Log(string msg)
        {
            if (!debug.Value) return;
            Debug.Log("[SolutionSubscriber]: " + msg);
        }
        private void LogWarning(string msg)
        {
            if (!debug.Value) return;
            Debug.LogWarning("[SolutionSubscriber]: " + msg);
        }
    }
}
