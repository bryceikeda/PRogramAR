using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    public class RuleType : MonoBehaviour, ISelectRuleTypeResponse
    {
        [SerializeField] ExpressionTree triggerExpressionTree;
        [SerializeField] ExpressionTree actionExpressionTree;
        [SerializeField] SelectRuleTypeEvent selectRuleTypeEvent;

        [SerializeField] ClauseRuntimeSet triggerRuntimeSet;
        [SerializeField] ClauseRuntimeSet actionRuntimeSet;

        [SerializeField] GameObject triggerList;
        [SerializeField] GameObject actionList;
        [SerializeField] GameObject backButton;

        void Start()
        {
            selectRuleTypeEvent.RegisterListener(this);
        }
        public void OnSelectRuleTypeEvent(string type)
        {
            foreach (ExpressionTree.ExpressionNode node in triggerExpressionTree.expressionNodes)
            {
                if (node.expressionNodeName.Equals("Default"))
                {
                    node.expressionNames.Clear();
                    node.nextExpressionNodes.Clear();

                    if (type.Equals("IfThen"))
                    {
                        node.expressionNames.Add("If");
                        node.nextExpressionNodes.Add("Objects");
                    }
                    else if (type.Equals("WhileDo"))
                    {
                        node.expressionNames.Add("While");
                        node.nextExpressionNodes.Add("Objects");
                    }
                    break;
                }
            }
            foreach (ExpressionTree.ExpressionNode node in actionExpressionTree.expressionNodes)
            {
                if (node.expressionNodeName.Equals("Default"))
                {
                    node.expressionNames.Clear();
                    node.nextExpressionNodes.Clear();

                    if (type.Equals("IfThen"))
                    {
                        node.expressionNames.Add("Then");
                        node.nextExpressionNodes.Add("Action");
                    }
                    else if (type.Equals("WhileDo"))
                    {
                        node.expressionNames.Add("Do");
                        node.nextExpressionNodes.Add("Action");
                    }
                    break;
                }
            }
            triggerRuntimeSet.Items.Clear();
            actionRuntimeSet.Items.Clear();
            triggerList.SetActive(true);
            actionList.SetActive(true);
            backButton.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
