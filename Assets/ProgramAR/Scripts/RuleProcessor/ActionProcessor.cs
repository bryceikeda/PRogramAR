 
using System.Collections.Generic;
using UnityEngine;
using ProgramAR.Pages;
using ProgramAR.Variables;

/// <summary>
///
/// </summary>

namespace RuleProcessor
{
    public class ActionProcessor : MonoBehaviour
    {
        [SerializeField] TriggerActionRules rules;
        [SerializeField] ZoneObjectRuntimeDictionary zoneObjects;
        [SerializeField] BoxObjectRuntimeDictionary boxObjects;
        [SerializeField] Transform worldTransform;
        private List<(string, ZoneObject)> actionList;

        public List<(string, ZoneObject)> EvaluateAction(Clause clause)
        {
            actionList = new List<(string, ZoneObject)>(); 
            if (clause.expressions[2].expressionName.Equals("all boxes in"))
            {
                // Move all boxes in a zoneInner to a zoneFinal

                // If zoneFinal exists check rest of rule
                if (zoneObjects.Items.TryGetValue(clause.expressions[5].expressionName[5] - '0', out ZoneObject zoneFinal))
                {
                    // if zoneInner exists, check rest of rule
                    if (zoneObjects.Items.TryGetValue(clause.expressions[3].expressionName[5] - '0', out ZoneObject zoneInner))
                    {
                        // For each box in zoneInner place in zoneFinal
                        foreach (string box in zoneInner.GetObjectsInside())
                        {
                            actionList.Add((box, zoneFinal));
                        }
                    }
                }
            }
            else if (clause.expressions[2].expressionName.Equals("all boxes not in"))
            {
                // Move all boxes not in a zone to a zone
                // last word is always "Zone #" 

                // If zoneFinal exists check rest of rule
                if (zoneObjects.Items.TryGetValue(clause.expressions[5].expressionName[5] - '0', out ZoneObject zoneFinal))
                {
                    // if zoneInner exists, check rest of rule
                    if (zoneObjects.Items.TryGetValue(clause.expressions[3].expressionName[5] - '0', out ZoneObject zoneInner))
                    {
                        var zoneInnerBoxes = zoneInner.GetObjectsInside();
                        // If a box is not in zoneInner, put into zoneFinal
                        foreach (string box in boxObjects.Items.Keys)
                        {
                            if (!zoneInnerBoxes.Contains(box))
                            {
                                actionList.Add((box, zoneFinal));
                            }
                        }
                    }
                }
            }
            else if (clause.expressions[2].expressionName.Equals("all boxes"))
            {
                // If zoneFinal exists
                if (zoneObjects.Items.TryGetValue(clause.expressions[4].expressionName[5] - '0', out ZoneObject zoneFinal))
                {
                    var zoneFinalBoxes = zoneFinal.GetObjectsInside();
                    // If a box is not in final already, put into zoneFinal
                    foreach (string box in boxObjects.Items.Keys)
                    {
                        if (!zoneFinalBoxes.Contains(box))
                        {
                            actionList.Add((box, zoneFinal));
                        }
                    }
                }
            }
            else
            {
                // Move box to inside a zone
                if (zoneObjects.Items.TryGetValue(clause.expressions[4].expressionName[5] - '0', out ZoneObject zoneFinal))
                {
                    // If the box is not already in zone, move it to zone
                    if (!zoneFinal.GetObjectsInside().Contains(clause.expressions[2].expressionName))
                    {
                        actionList.Add((clause.expressions[2].expressionName, zoneFinal));
                    }
                }
            }
            return actionList; 
        }

        public List<(string, ZoneObject)> EvaluateAction(int index)
        {
            actionList = new List<(string, ZoneObject)>();
            if (rules.pairs.Count > index)
            {
                // Check the actions
                for (int k = 0; k < rules.pairs[index].action.Count; k++)
                {
                    var clause = rules.pairs[index].action[k];
                    if (clause.expressions[2].expressionName.Equals("all boxes in"))
                    {
                        // Move all boxes in a zoneInner to a zoneFinal

                        // If zoneFinal exists check rest of rule
                        if (zoneObjects.Items.TryGetValue(clause.expressions[5].expressionName[5] - '0', out ZoneObject zoneFinal))
                        {
                            // if zoneInner exists, check rest of rule
                            if (zoneObjects.Items.TryGetValue(clause.expressions[3].expressionName[5] - '0', out ZoneObject zoneInner))
                            {
                                // For each box in zoneInner place in zoneFinal
                                foreach (string box in zoneInner.GetObjectsInside())
                                {
                                    actionList.Add((box, zoneFinal));
                                }
                            }
                        }
                    }
                    else if (clause.expressions[2].expressionName.Equals("all boxes not in"))
                    {
                        // Move all boxes not in a zone to a zone
                        // last word is always "Zone #" 

                        // If zoneFinal exists check rest of rule
                        if (zoneObjects.Items.TryGetValue(clause.expressions[5].expressionName[5] - '0', out ZoneObject zoneFinal))
                        {
                            // if zoneInner exists, check rest of rule
                            if (zoneObjects.Items.TryGetValue(clause.expressions[3].expressionName[5] - '0', out ZoneObject zoneInner))
                            {
                                var zoneInnerBoxes = zoneInner.GetObjectsInside();
                                // If a box is not in zoneInner, put into zoneFinal
                                foreach (string box in boxObjects.Items.Keys)
                                {
                                    if (!zoneInnerBoxes.Contains(box))
                                    {
                                        actionList.Add((box, zoneFinal));
                                    }
                                }
                            }
                        }
                    }
                    else if (clause.expressions[2].expressionName.Equals("all boxes"))
                    {
                        // If zoneFinal exists
                        if (zoneObjects.Items.TryGetValue(clause.expressions[4].expressionName[5] - '0', out ZoneObject zoneFinal))
                        {
                            var zoneFinalBoxes = zoneFinal.GetObjectsInside();
                            // If a box is not in final already, put into zoneFinal
                            foreach (string box in boxObjects.Items.Keys)
                            {
                                if (!zoneFinalBoxes.Contains(box))
                                {
                                    actionList.Add((box, zoneFinal));
                                }
                            }
                        }
                    }
                    else
                    {
                        // Move box to inside a zone
                        if (zoneObjects.Items.TryGetValue(clause.expressions[4].expressionName[5] - '0', out ZoneObject zoneFinal))
                        {
                            // If the box is not already in zone, move it to zone
                            if (!zoneFinal.GetObjectsInside().Contains(clause.expressions[2].expressionName))
                            {
                                actionList.Add((clause.expressions[2].expressionName, zoneFinal));
                            }
                        }
                    }
                }
            }

            return actionList;
        }
    }
}
