using ProgramAR.Pages;
using ProgramAR.Variables;
using RosMessageTypes.Geometry;
using RosMessageTypes.TaskPlanner;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

namespace RuleProcessor
{ 


    [RequireComponent(typeof(ActionProcessor))]
    [RequireComponent(typeof(TriggerProcessor))]
    public class RuleManager : MonoBehaviour
    {
        enum Stage { IDLE, EVALUATE_TRIGGER, EVALUATE_ACTION, EXECUTE_ACTION};

        [SerializeField] ActionProcessor actionProcessor;
        [SerializeField] TriggerProcessor triggerProcessor;
        [SerializeField] TriggerActionRules rules;
        
        [SerializeField] ActionCompleteEvent actionCompleteEvent; 
        [SerializeField] ActionStatusVariable actionStatus;
        [SerializeField] UpdatePlanningSceneEvent updatePlanningSceneEvent;
        [SerializeField] HighlightRuleEvent highlightRuleEvent;
        [SerializeField] IndicateIsRunningEvent indicateIsRunningEvent;
        [SerializeField] UpdateRunStatusEvent updateRunStatusEvent; 
        ROSConnection m_Ros;
        public string topicName = "/unity/query_planner";
        [SerializeField] PlanStatusEvent planStatusEvent; 
        [SerializeField] int currentRule = 0;
        [SerializeField] int currentRuleHighlight = 0;
        [SerializeField] IntegerVariable currentAction;
        private List<(string, ZoneObject)> actionList;
        Stage currentStage = Stage.IDLE;

        PickPlaceMsg actionMsg;
        [SerializeField] List<CheckPointInsideBox> bounds;
        [SerializeField] BoolVariable debug; 

        private float nextActionTime = 0.0f;
        public float period = 10f;


        private void Start()
        {
            currentAction.Value = 0; 
        }
        private void OnValidate()
        {
            m_Ros = ROSConnection.GetOrCreateInstance();
            m_Ros.RegisterPublisher<PickPlaceMsg>(topicName);
            actionMsg = new PickPlaceMsg(); 
            actionList = new List<(string, ZoneObject)>();
            actionProcessor = GetComponent<ActionProcessor>();
            triggerProcessor = GetComponent<TriggerProcessor>(); 
        }

        private void Update()
        {
            switch (currentStage)
            {
                case Stage.IDLE:
                    // Do nothing
                    break; 
                // Check if the trigger is valid, if not, then check the next rule 
                case Stage.EVALUATE_TRIGGER:
                    if (triggerProcessor.EvaluateTrigger(currentRule))
                    {
                        updateRunStatusEvent.Raise("Running Program");
                        currentStage = Stage.EVALUATE_ACTION;
                        Log("EVALUATE TRIGGER " + currentRule + " TRUE -> EVALUATE_ACTION " + currentRule);
                    }
                    else
                    {
                        updateRunStatusEvent.Raise("Running Program");
                        indicateIsRunningEvent.Raise(currentRule, false);
                        var evaluated = currentRule;
                        currentRule++;
                        if (currentRule >= rules.pairs.Count)
                        {
                            currentRule = 0;
                        }
                        Log("EVALUATE TRIGGER  " + evaluated + " FALSE -> EVALUATE_TRIGGER " + currentRule);

                    }
                    break;
                // Check if there are valid actions to perform 
                case Stage.EVALUATE_ACTION:

                    actionList = actionProcessor.EvaluateAction(currentRule);

                    if (actionList.Count == 0)
                    {
                        currentStage = Stage.EVALUATE_TRIGGER;
                            

                        // If this is an If statement, go to the next rule, while stays the same
                        if (triggerProcessor.GetOperator(currentRule).Equals("If"))
                        {
                            var evaluated = currentRule; 
                            currentRule++;
                            if (currentRule >= rules.pairs.Count)
                            {
                                currentRule = 0;
                            }
                            Log("EVALUATE ACTION " + evaluated + " FALSE -> EVALUATE_TRIGGER " + currentRule);
                        }
                        else
                        {
                            Log("EVALUATE ACTION " + currentRule + " FALSE -> EVALUATE_TRIGGER " + currentRule);
                        }
                    }
                    else
                    {
                        currentStage = Stage.EXECUTE_ACTION;
                        Log("EVALUATE ACTION " + currentRule + " TRUE -> EXECUTE_ACTION: " + currentRule);
                    }
                    break;
                case Stage.EXECUTE_ACTION:
                    if (actionStatus.Done == true)
                    {
                        if (currentAction.Value < actionList.Count)
                        {
                            Log("EXECUTE ACTION rule number " + currentRule + " action " + currentAction.Value + "/" + actionList.Count);
                            if (actionList[currentAction.Value].Item2.GetObjectsInside().Count == 0)
                            {
                                updateRunStatusEvent.Raise("Rule " + (currentRule + 1) + ": Executing");
                                actionStatus.Done = false;
                                StartCoroutine(SendCommand());
                            }
                            else
                            {
                                planStatusEvent.Raise("Rule " + (currentRule + 1) + ": Zone Full");
                                updateRunStatusEvent.Raise("Rule " + (currentRule + 1) + ": Zone Full"); 
                            }
                            indicateIsRunningEvent.Raise(currentRule, true);
                        }
                        else if (currentAction.Value >= actionList.Count)
                        {
                            currentAction.Value = 0;

                            // If this is an If statement, go to the next rule, while stays the same
                            if (triggerProcessor.GetOperator(currentRule).Equals("If"))
                            {
                                indicateIsRunningEvent.Raise(currentRule, false);
                                var evaluated = currentRule;
                                currentRule++;
                                if (currentRule >= rules.pairs.Count)
                                {
                                    currentRule = 0;
                                }
                                Log("EXECUTE ACTION " + evaluated + " DONE -> EVALUATE_TRIGGER " + currentRule);
                            }
                            else
                            {
                                Log("EXECUTE ACTION " + currentRule + " DONE -> EVALUATE_TRIGGER " + currentRule);
                            }

                            currentStage = Stage.EVALUATE_TRIGGER;
                        }
                    }
                    break;
                
            }

            // highlight rules if they are valid
            if (rules.pairs.Count != 0)
            {
                if (Time.time > nextActionTime)
                {
                    nextActionTime += period;
                    if (triggerProcessor.EvaluateTrigger(currentRuleHighlight))
                    {
                        if (actionProcessor.EvaluateAction(currentRuleHighlight).Count == 0)
                        {
                            highlightRuleEvent.Raise(currentRuleHighlight, true, false);
                        }
                        else
                        {
                            highlightRuleEvent.Raise(currentRuleHighlight, true, true);
                        }
                    }
                    else
                    {
                        if (actionProcessor.EvaluateAction(currentRuleHighlight).Count == 0)
                        {
                            highlightRuleEvent.Raise(currentRuleHighlight, false, false);
                        }
                        else
                        {
                            highlightRuleEvent.Raise(currentRuleHighlight, false, true);
                        }
                    }
                    currentRuleHighlight++;
                    if (currentRuleHighlight >= rules.pairs.Count)
                    {
                        currentRuleHighlight = 0;
                    }
                }
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

            var temp2 = outPos.To<FLU>();
            return new Vector3(temp2.x, temp2.y, temp2.z);
        }


        IEnumerator SendCommand()
        {
            updatePlanningSceneEvent.Raise(actionList[currentAction.Value].Item1);
            planStatusEvent.Raise("Scanning Environment please wait...");
            yield return new WaitForSeconds(20);
            planStatusEvent.Raise("Rule " + (currentRule + 1) + ": Command Sent please wait...");
            updateRunStatusEvent.Raise("Rule " + (currentRule + 1) + ": Command Sent");

            Log("Send Command ActionList size: " + actionList.Count );

            var place = ZonePlacementPosition(actionList[currentAction.Value].Item2);

            actionMsg.task_type = PickPlaceMsg.PICK_AND_PLACE;
            actionMsg.grasp_frame = PickPlaceMsg.VERTICAL;
            actionMsg.execute = true;
            actionMsg.object_name = actionList[currentAction.Value].Item1;
            actionMsg.place_pose = new PoseMsg(new PointMsg(place.x, place.y, place.z), new QuaternionMsg(0, 0, 0, 1f)); 

            m_Ros.Publish(topicName, actionMsg);
            
            Log("[ActionProcessor]: Published Command");
        }

        public void OnRunProgram()
        {
            if (currentStage == Stage.IDLE)
            {
                if (rules.pairs.Count != 0)
                {
                    Log("Running Program");
                    currentStage = Stage.EVALUATE_TRIGGER;
                    actionStatus.Done = true;
                    currentAction.Value = 0;
                    currentRule = 0;
                }
            }
            else
            {
                indicateIsRunningEvent.Raise(currentRule, false);
                currentStage = Stage.IDLE; 
            }
        }

        private void OnApplicationQuit()
        {
            actionStatus.Done = true;
        }

        private void Log(string msg)
        {
            if (!debug.Value) return;
            Debug.Log("[RuleManager]: " + msg);
        }
        private void LogWarning(string msg)
        {
            if (!debug.Value) return;
            Debug.LogWarning("[RuleManager]: " + msg);
        }
    }
}
