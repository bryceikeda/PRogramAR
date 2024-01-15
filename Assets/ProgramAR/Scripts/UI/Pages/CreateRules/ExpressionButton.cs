using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

namespace ProgramAR.Pages
{
    public class ExpressionButton : MonoBehaviour
    {
        [SerializeField] ButtonConfigHelper buttonConfigHelper;
        [SerializeField] Interactable interactable;
        [SerializeField] PressableButtonHoloLens2 pressableButton;
        [SerializeField] ExpressionSelectedEvent buttonEvent;
        [SerializeField] int buttonRow = -1;
        Expression expression;

        private void Awake()
        {
            expression = new Expression("", "", "");
        }

        public void OnClick()
        {
            buttonEvent.Raise(buttonRow, expression);
        }

        public void SetExpression(string expressionNodeName, string expressionName, string nextExpressionNode, string expressionIconName)
        {
            expression.SetExpression(expressionNodeName, expressionName, nextExpressionNode);
            buttonConfigHelper.MainLabelText = expressionName;
            interactable.enabled = true;
            pressableButton.enabled = true;
            if (!expressionIconName.Equals("None"))
            {
                buttonConfigHelper.SetQuadIconByName(expressionIconName);
            }
            
        }

        public void Disable()
        {
            buttonConfigHelper.MainLabelText = "";
            interactable.IsToggled = false;
            interactable.enabled = false;
            pressableButton.enabled = false;
        }

        public void SelectButton()
        {
            interactable.IsToggled = true;
        }

        public void Invoke()
        {
            interactable.OnClick.Invoke();
            //interactable.TriggerOnClick(true);  
        }

        public Expression GetExpression()
        {
            return expression;
        }

        private void OnEnable()
        {
            interactable.OnClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            interactable.OnClick.RemoveListener(OnClick);
        }

        private void OnValidate()
        {
            if (buttonConfigHelper == null)
            {
                buttonConfigHelper = GetComponent<ButtonConfigHelper>();
            }
            if (interactable == null)
            {
                interactable = GetComponent<Interactable>();
            }
            if (pressableButton == null)
            {
                pressableButton = GetComponent<PressableButtonHoloLens2>();
            }
            if (buttonRow == -1)
            {
                buttonRow = gameObject.transform.parent.GetSiblingIndex();
            }
        }
    }
}