using UnityEngine;
using System.Collections.Generic;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Set/TriggerActionRules"), System.Serializable]
    public class TriggerActionRules : ScriptableObject
    {
        public List<TriggerActionPair> pairs;

        public void AddPair(ClauseRuntimeSet trigger, ClauseRuntimeSet action)
        {
            var newPair = new TriggerActionPair
            {

                trigger = new List<Clause>(trigger.Items),
                action = new List<Clause>(action.Items)
            };
            pairs.Add(newPair);
        }

        public void EditPair(int selectedTAP, ClauseRuntimeSet trigger, ClauseRuntimeSet action)
        {
            pairs[selectedTAP].trigger.Clear();
            pairs[selectedTAP].action.Clear();
            pairs.RemoveAt(selectedTAP);

            var newPair = new TriggerActionPair
            {

                trigger = new List<Clause>(trigger.Items),
                action = new List<Clause>(action.Items)
            };

            pairs.Insert(selectedTAP, newPair);
        }

        public void Copy(int selectedTAP)
        {
            var triggerList = new List<Clause>();
            var actionList = new List<Clause>();

            for (int i = 0; i < pairs[selectedTAP].trigger.Count; i++)
            {
                var triggerExpressions = new List<Expression>();
                var actionExpressions = new List<Expression>();
                foreach (Expression expression in pairs[selectedTAP].trigger[i].expressions)
                {
                    triggerExpressions.Add(expression.Copy());
                }
                foreach (Expression expression in pairs[selectedTAP].action[i].expressions)
                {
                    actionExpressions.Add(expression.Copy());
                }
                triggerList.Add(new Clause(triggerExpressions));
                actionList.Add(new Clause(actionExpressions));
            }
            

            var newPair = new TriggerActionPair
            {
                trigger = triggerList,
                action = actionList
                
            };

            pairs.Add(newPair);
        }

        //trigger = new List<Clause>(pairs[selectedTAP].trigger),
        //action = new List<Clause>(pairs[selectedTAP].action)

        public string GetActionText(int index)
        {
            string name = "";
            foreach (Clause clause in pairs[index].action)
            {
                name += clause.GetClauseName() + "<br>";
            }
            return name;
        }

        public string GetTriggerText(int index)
        {
            string name = "";
            foreach (Clause clause in pairs[index].trigger)
            {
                name += clause.GetClauseName() + "<br>";
            }
            return name;
        }

        public void ChangePriority(int initialPriority, int newPriority)
        {
            if (!(newPriority > pairs.Count || newPriority < 0))
            {
                TriggerActionPair tap = pairs[initialPriority];
                pairs.RemoveAt(initialPriority);
                pairs.Insert(newPriority, tap);
            }
        }

        public void DeletePair(int pairIndex)
        {
            pairs[pairIndex].trigger.Clear();
            pairs[pairIndex].action.Clear();
            pairs.RemoveAt(pairIndex);
        }
    }
}
