 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.MoveitTaskConstructor;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Moveit;
using ProgramAR.Variables;
/// <summary>
///
/// </summary>

namespace RuleProcessor
{
    public class SolutionSubscriberArticulation : MonoBehaviour
    {
        public static readonly string[] LinkNames =
        { "world/fetch/base_link/torso_lift_link", "/shoulder_pan_link", "/shoulder_lift_link", "/upperarm_roll_link", "/elbow_flex_link", "/forearm_roll_link", "/wrist_flex_link", "wrist_roll_link" };

        [SerializeField] ArticulationBody m_RightGripper;
        [SerializeField] ArticulationBody m_LeftGripper;

        [SerializeField]
        JointController[] m_OrderedJoints;

        // Robot Joints
        [SerializeField] List<ArticulationBody> m_JointArticulationBodies;
        const float k_JointAssignmentWait = 0.01f;
        const float k_PoseAssignmentWait = 0.3f;
        public GameObject m_Fetch; 

        ROSConnection m_Ros;

        public string planSolutionTopicName = "/task_planner/pick_and_place/solution";
        public string executeSolutionResultTopicName = "/execute_task_solution/result";

        public SolutionScriptableObject sol;

        [SerializeField] ActionStatusVariable solCallback; 

        void Start()
        {
            // Get ROS connection static instance
            m_Ros = ROSConnection.GetOrCreateInstance();
            m_Ros.Subscribe<SolutionMsg>(planSolutionTopicName, SolutionCallback);
            m_Ros.Subscribe<ExecuteTaskSolutionActionResult>(executeSolutionResultTopicName, SolutionResultCallback);
        }
        


        void SolutionResultCallback(ExecuteTaskSolutionActionResult result)
        {
            solCallback.Done = false;
            solCallback.ErrorCode = result.result.error_code; 

            switch (result.result.error_code.val)
            {
                case MoveItErrorCodesMsg.SUCCESS:
                    Debug.Log("SUCCESS");
                    break;
                case MoveItErrorCodesMsg.FAILURE:
                    Debug.LogWarning("FAILURE");
                    break;
                case MoveItErrorCodesMsg.PLANNING_FAILED:
                    Debug.LogWarning("PLANNING_FAILED");
                    break;
                case MoveItErrorCodesMsg.INVALID_MOTION_PLAN:
                    Debug.LogWarning("INVALID_MOTION_PLAN");
                    break;
                case MoveItErrorCodesMsg.MOTION_PLAN_INVALIDATED_BY_ENVIRONMENT_CHANGE:
                    Debug.LogWarning("FAILURE");
                    break;
                case MoveItErrorCodesMsg.CONTROL_FAILED:
                    Debug.LogWarning("CONTROL_FAILED");
                    break;
                case MoveItErrorCodesMsg.UNABLE_TO_AQUIRE_SENSOR_DATA:
                    Debug.LogWarning("UNABLE_TO_AQUIRE_SENSOR_DATA");
                    break;
                case MoveItErrorCodesMsg.TIMED_OUT:
                    Debug.LogWarning("TIMED_OUT");
                    break;
                case MoveItErrorCodesMsg.PREEMPTED:
                    Debug.LogWarning("PREEMPTED");
                    break;
                case MoveItErrorCodesMsg.START_STATE_IN_COLLISION:
                    Debug.LogWarning("START_STATE_IN_COLLISION");
                    break;
                case MoveItErrorCodesMsg.START_STATE_VIOLATES_PATH_CONSTRAINTS:
                    Debug.LogWarning("START_STATE_VIOLATES_PATH_CONSTRAINTS");
                    break;
                case MoveItErrorCodesMsg.GOAL_IN_COLLISION:
                    Debug.LogWarning("GOAL_IN_COLLISION");
                    break;
                case MoveItErrorCodesMsg.GOAL_VIOLATES_PATH_CONSTRAINTS:
                    Debug.LogWarning("GOAL_VIOLATES_PATH_CONSTRAINTS");
                    break;
                case MoveItErrorCodesMsg.GOAL_CONSTRAINTS_VIOLATED:
                    Debug.LogWarning("GOAL_CONSTRAINTS_VIOLATED");
                    break;
                case MoveItErrorCodesMsg.INVALID_GROUP_NAME:
                    Debug.LogWarning("INVALID_GROUP_NAME");
                    break;
                case MoveItErrorCodesMsg.INVALID_GOAL_CONSTRAINTS:
                    Debug.LogWarning("INVALID_GOAL_CONSTRAINTS");
                    break;
                case MoveItErrorCodesMsg.INVALID_ROBOT_STATE:
                    Debug.LogWarning("INVALID_ROBOT_STATE");
                    break;
                case MoveItErrorCodesMsg.INVALID_LINK_NAME:
                    Debug.LogWarning("INVALID_LINK_NAME");
                    break;
                case MoveItErrorCodesMsg.INVALID_OBJECT_NAME:
                    Debug.LogWarning("INVALID_OBJECT_NAME");
                    break;
                case MoveItErrorCodesMsg.FRAME_TRANSFORM_FAILURE:
                    Debug.LogWarning("FRAME_TRANSFORM_FAILURE");
                    break;
                case MoveItErrorCodesMsg.COLLISION_CHECKING_UNAVAILABLE:
                    Debug.LogWarning("COLLISION_CHECKING_UNAVAILABLE");
                    break;
                case MoveItErrorCodesMsg.ROBOT_STATE_STALE:
                    Debug.LogWarning("ROBOT_STATE_STALE");
                    break;
                case MoveItErrorCodesMsg.SENSOR_INFO_STALE:
                    Debug.LogWarning("SENSOR_INFO_STALE");
                    break;
                case MoveItErrorCodesMsg.COMMUNICATION_FAILURE:
                    Debug.LogWarning("COMMUNICATION_FAILURE");
                    break;
                case MoveItErrorCodesMsg.NO_IK_SOLUTION:
                    Debug.LogWarning("NO_IK_SOLUTION");
                    break;
                default:
                    Debug.LogError("Invalid Error Code");
                    break;
            }
        }

        void SolutionCallback(SolutionMsg solution)
        {
            sol.solution.Add(solution);
        }

        void TrajectoryResponse(SolutionMsg solution)
        {
            if (solution.sub_trajectory.Length > 0)
            {
                StartCoroutine(ExecuteTrajectories(solution));
            }
            else
            {
                Debug.LogError("No trajectory returned from MoverService.");
            }
        }

        IEnumerator ExecuteTrajectoriesExact(SolutionMsg response)
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

                                var leftDrive = m_LeftGripper.xDrive;
                                var rightDrive = m_RightGripper.xDrive;

                                leftDrive.target = (float)jointPositions[0];
                                rightDrive.target = (float)jointPositions[1];

                                m_LeftGripper.xDrive = leftDrive;
                                m_RightGripper.xDrive = rightDrive;
                                // Wait for robot to achieve pose for all joint assignments
                                yield return new WaitForSeconds(k_JointAssignmentWait);
                            }
                        }

                        // Wait for the robot to achieve the final pose from joint assignment
                        yield return new WaitForSeconds(k_PoseAssignmentWait);
                    }
                }
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
                                for (var joint = 0; joint < m_JointArticulationBodies.Count; joint++)
                                {
                                    if (joint == 0)
                                    {
                                        var joint1XDrive = m_JointArticulationBodies[joint].xDrive;
                                        joint1XDrive.target = (float)jointPositions[joint];
                                        m_JointArticulationBodies[joint].xDrive = joint1XDrive;
                                    }
                                    else
                                    {
                                        var joint1XDrive = m_JointArticulationBodies[joint].xDrive;
                                        joint1XDrive.target = (float)jointPositions[joint] * Mathf.Rad2Deg;
                                        m_JointArticulationBodies[joint].xDrive = joint1XDrive;
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

                                var leftDrive = m_LeftGripper.xDrive;
                                var rightDrive = m_RightGripper.xDrive;

                                leftDrive.target = (float)jointPositions[0];
                                rightDrive.target = (float)jointPositions[1];

                                m_LeftGripper.xDrive = leftDrive;
                                m_RightGripper.xDrive = rightDrive;
                                // Wait for robot to achieve pose for all joint assignments
                                yield return new WaitForSeconds(k_JointAssignmentWait);
                            }
                        }

                        // Wait for the robot to achieve the final pose from joint assignment
                        yield return new WaitForSeconds(k_PoseAssignmentWait);
                    }
                }
            }
        }
    }
}
