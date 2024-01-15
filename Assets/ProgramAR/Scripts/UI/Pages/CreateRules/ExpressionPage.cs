using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using ProgramAR.Events;
using ProgramAR.Variables; 

namespace ProgramAR.Pages
{
    public class ExpressionPage : MonoBehaviour, IExpressionSelectedResponse, IAddClauseResponse, IEditClauseEventResponse, ISaveClauseResponse, IBackResponse
    {
        [SerializeField] ExpressionSelectedEvent expressionSelectedEvent;
        [SerializeField] AddClauseEvent addClauseEvent;
        [SerializeField] EditClauseEvent editClauseEvent;
        [SerializeField] BackEvent backEvent;
        [SerializeField] SaveClauseEvent saveClauseEvent; 
        [SerializeField] ShowClauseListEvent showClauseListEvent;
        [SerializeField] ToggleSaveVisibilityEvent toggleSaveVisibilityEvent;
        [SerializeField] HideClauseListEvent hideClauseListEvent;

        [SerializeField] GameObject expressionButtons; 

        [SerializeField] BoolVariable defaultClauseAdded;

        [SerializeField] ExpressionTree expressionTree;
        [SerializeField] ExpressionTree errorTree; 
        [SerializeField] ClauseRuntimeSet clauses;

        [SerializeField] ExpressionManager[] expressionManagers;

        public int loadedClause = -1;
        public bool isActive = false;
        private void OnValidate()
        {
            if (expressionManagers == null)
            {
                expressionManagers = expressionButtons.GetComponentsInChildren<ExpressionManager>();
            }
        }

        public void OnAddClauseEvent()
        {
            isActive = true;

            expressionButtons.SetActive(true);
            loadedClause = -1;
            
            expressionManagers[0].gameObject.SetActive(true);

            if (!defaultClauseAdded.Value)
            {
                expressionManagers[0].SetInvokeExpressions(0, expressionTree.expressionNodes[0]);
            }
            else
            {
                if (expressionTree.expressionNodes[1].expressionNames.Count == 1)
                {
                    expressionManagers[0].SetInvokeExpressions(0, expressionTree.expressionNodes[1]);
                }
                else
                {
                    expressionManagers[0].SetExpressions(expressionTree.expressionNodes[1]);
                }
            }
            toggleSaveVisibilityEvent.Raise(false, "OnAddClauseEvent");
            hideClauseListEvent.Raise(); 
        }

        public void OnEditClauseEvent(int selectedClause)
        {
            isActive = true;
            expressionButtons.SetActive(true);
            loadedClause = selectedClause;

            List<Expression> expressions = clauses.Items[loadedClause].GetExpressions();

            for (int selection = 0; selection < expressions.Count; selection++)
            {
                expressionManagers[selection].gameObject.SetActive(true);
                expressionManagers[selection].SetSelectExpressions(expressions[selection].GetExpressionName(),
                                                            GetNextExpressionNodes(expressions[selection].GetExpressionNodeName()));
            }
        }

        public void OnExpressionSelectedEvent(int selectedRow, Expression selectedExpression)
        {
            // This gets set by the pressing new trigger or new action
            if (isActive)
            {
                UpdatePage(selectedRow, selectedExpression.GetNextExpressionNode());
            }
        }
        public void OnBackEvent()
        {
            // This gets set by the pressing new trigger or new action
            if (isActive)
            {
                HidePage();
                isActive = false;
                showClauseListEvent.Raise();
            }
        }

        public void OnSaveClauseEvent()
        {
            // This gets set by the pressing new trigger or new action
            if (isActive)
            {
                Clause clause = new Clause();
                // Create a new clause object, if it is a loaded clause, edit that one
                if (loadedClause == -1)
                {
                    clauses.Add(clause);
                }
                else
                {
                    clause = clauses.Items[loadedClause];
                }

                // Add clause description to list
                for (int index = 0; index < expressionManagers.Length; index++)
                {
                    // If thereare no more expressions, break the loop
                    if (expressionManagers[index].GetSelectedExpressionIndex() == -1)
                    {
                        break;
                    }
                    clause.AddExpression(index, expressionManagers[index].GetSelectedExpression().Copy());
                }

                // Make sure the default clause is the first one
                if (clauses.Items.Count > 1)
                {
                    if (!defaultClauseAdded.Value)
                    {
                        var temp = clause;
                        clauses.Remove(clause);
                        clauses.Items.Insert(0, temp);
                        defaultClauseAdded.FlipValue();
                    }
                }
                else if (!defaultClauseAdded.Value) 
                {
                    defaultClauseAdded.FlipValue();
                }

                HidePage();
                isActive = false;

                showClauseListEvent.Raise();
            }
        }

        public void UpdatePage(int selectedRow, string nextExpressionNode)
        {
            if (!nextExpressionNode.Equals("Error"))
            {
                if (!nextExpressionNode.Equals("Save"))
                {
                    // Disable all expressions after this row
                    for (int i = (selectedRow + 2); i < expressionManagers.Length; i++)
                    {
                        expressionManagers[i].gameObject.SetActive(false);
                    }

                    // Make next row of expressions active and set their properties
                    expressionManagers[selectedRow + 1].gameObject.SetActive(true);

                    // Automatically select the next expression if it is the only one available
                    var nextExpressions = GetNextExpressionNodes(nextExpressionNode);
                    toggleSaveVisibilityEvent.Raise(false, "UpdatePage");

                    if (nextExpressions.expressionNames.Count == 1 && !nextExpressions.expressionNodeName.Equals("Error"))
                    {
                        expressionManagers[selectedRow + 1].SetInvokeExpressions(0, GetNextExpressionNodes(nextExpressionNode));
                    }
                    else
                    {
                        expressionManagers[selectedRow + 1].SetExpressions(GetNextExpressionNodes(nextExpressionNode));
                    }
                }
                else
                {
                    toggleSaveVisibilityEvent.Raise(true, "ExpressionPage");
                }
            }
        }

        private ExpressionTree.ExpressionNode GetNextExpressionNodes(string nextExpressionNode)
        {
            foreach (ExpressionTree.ExpressionNode node in expressionTree.expressionNodes)
            {
                if (node.expressionNodeName == nextExpressionNode)
                {
                    if (node.expressionNames.Count == 0)
                    {
                        errorTree.expressionNodes[0].expressionNames[0] = "Error: No " + nextExpressionNode;
                        return errorTree.expressionNodes[0]; 
                    }
                    return node;
                }
            }
            LogWarning("Next expression node does not exist");
            return errorTree.expressionNodes[0];
        }
        private void HidePage()
        {
            foreach (ExpressionManager manager in expressionManagers)
            {
                manager.ResetSelection();
                manager.gameObject.SetActive(false);
            }

            expressionButtons.SetActive(false);
        }
        private void OnEnable()
        {
            expressionSelectedEvent.RegisterListener(this);
            addClauseEvent.RegisterListener(this);
            editClauseEvent.RegisterListener(this);
            saveClauseEvent.RegisterListener(this);
            backEvent.RegisterListener(this);
            HidePage();
            isActive = false;
            showClauseListEvent.Raise();
        }

        private void OnDisable()
        {
            expressionSelectedEvent.UnregisterListener(this);
            addClauseEvent.UnregisterListener(this);
            editClauseEvent.UnregisterListener(this);
            saveClauseEvent.UnregisterListener(this);
            backEvent.UnregisterListener(this);
        }

        private void Log(string _msg)
        {
            Debug.Log("[ExpressionPage]: " + _msg);
        }

        private void LogWarning(string _msg)
        {
            Debug.LogWarning("[ExpressionPage]: " + _msg);
        }
    }
}
