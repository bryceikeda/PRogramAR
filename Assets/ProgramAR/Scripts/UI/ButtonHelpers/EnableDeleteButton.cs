using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using ProgramAR.Events;
using ProgramAR.Variables; 

namespace ProgramAR.ButtonHelpers
{
    [RequireComponent(typeof(ButtonConfigHelper))]
    public class EnableDeleteButton : MonoBehaviour
    {
        [SerializeField] BoolVariable isEnabled;
        [SerializeField] ButtonConfigHelper buttonConfigHelper;
        [SerializeField] GenericEvent enableButtonsEvent;

        public string offText;
        public string onText;

        private void OnValidate()
        {
            buttonConfigHelper = GetComponent<ButtonConfigHelper>();
        }

        // Start is called before the first frame update
        void Awake()
        {
            buttonConfigHelper.OnClick.AddListener(OnClick);
            buttonConfigHelper.MainLabelText = offText;
        }

        public void OnClick()
        {
            isEnabled.FlipValue();
            if (isEnabled.Value)
            {
                buttonConfigHelper.MainLabelText = onText;
            }
            else
            {
                buttonConfigHelper.MainLabelText = offText;
            }
            enableButtonsEvent.Raise();
        }

        private void OnDisable()
        {
            isEnabled.SetValue(false);
            if (buttonConfigHelper != null)
            {
                buttonConfigHelper.OnClick.RemoveListener(OnClick);
            }
        }
    }
}