using Microsoft.MixedReality.Toolkit.Utilities;
using ProgramAR.Menu; 
using UnityEngine;

namespace ProgramAR.Pages
{
    [RequireComponent(typeof(ToggleCollection))]
    [RequireComponent(typeof(GridObjectCollection))]
    public class ExpressionManager : MonoBehaviour
    {
        [SerializeField] ExpressionButton[] expressionButtons;
        [SerializeField] ToggleCollection toggleCollection;
        [SerializeField] GridObjectCollection objectCollection;

        private void OnValidate()
        {
            if (expressionButtons == null)
            {
                expressionButtons = GetComponentsInChildren<ExpressionButton>();
            }
            if (toggleCollection == null)
            {
                toggleCollection = GetComponent<ToggleCollection>();
            }
            if (objectCollection == null)
            {
                objectCollection = GetComponent<GridObjectCollection>();
            }
        }


        public void SetExpressions(ExpressionTree.ExpressionNode node, int startIndex = 0)
        {
            toggleCollection.Reset();
            for (int k = startIndex; k < expressionButtons.Length; k++)
            {
                var button = expressionButtons[k];
                if (k < node.expressionNames.Count)
                {
                    button.gameObject.SetActive(true);
                    button.SetExpression(node.expressionNodeName, node.expressionNames[k], node.nextExpressionNodes[k], node.expressionIconName[k]);
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
            objectCollection.UpdateCollection();
        }

        public void SetSelectExpressions(string selection, ExpressionTree.ExpressionNode node, int startIndex = 0)
        {
            toggleCollection.Reset();
            for (int k = startIndex; k < expressionButtons.Length; k++)
            {
                var button = expressionButtons[k];

                if (k < node.expressionNames.Count)
                {
                    button.gameObject.SetActive(true);
                    button.SetExpression(node.expressionNodeName, node.expressionNames[k], node.nextExpressionNodes[k], node.expressionIconName[k]);

                    if (node.expressionNames[k].Equals(selection))
                    {
                        button.Invoke();
                        toggleCollection.SetSelection(k);
                    }
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
            objectCollection.UpdateCollection();
        }

        public void SetInvokeExpressions(int selection, ExpressionTree.ExpressionNode node, int startIndex = 0)
        {
            toggleCollection.Reset();
            for (int k = startIndex; k < expressionButtons.Length; k++)
            {
                ExpressionButton button = expressionButtons[k];

                if (k < node.expressionNames.Count)
                {
                    button.gameObject.SetActive(true);
                    button.SetExpression(node.expressionNodeName, node.expressionNames[k], node.nextExpressionNodes[k], node.expressionIconName[k]);

                    if (k == selection)
                    {
                        button.Invoke();
                        toggleCollection.SetSelection(k);
                    }
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
            objectCollection.UpdateCollection();
        }
        public void SetSelectExpressions(int selection, ExpressionTree.ExpressionNode node, int startIndex = 0)
        {
            toggleCollection.Reset();
            for (int k = startIndex; k < expressionButtons.Length; k++)
            {
                var button = expressionButtons[k];

                if (k < node.expressionNames.Count)
                {
                    button.gameObject.SetActive(true);
                    button.SetExpression(node.expressionNodeName, node.expressionNames[k], node.nextExpressionNodes[k], node.expressionIconName[k]);

                    if (k == selection)
                    {
                        toggleCollection.SelectedIndex = k;
                        button.SelectButton();
                    }
                }
                else
                {
                    button.gameObject.SetActive(false);
                }
            }
            objectCollection.UpdateCollection();
        }

        public int GetSelectedExpressionIndex()
        {
            return toggleCollection.SelectedIndex;
        }
        public Expression GetSelectedExpression()
        {
            return expressionButtons[toggleCollection.SelectedIndex].GetExpression();
        }
        public void ResetSelection()
        {
            toggleCollection.Reset(); 
        }
    }
}