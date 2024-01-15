using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace ProgramAR
{
    namespace Pages
    {
        [RequireComponent(typeof(ButtonConfigHelper))]
        public class ButtonLabelToggle : MonoBehaviour
        {
            ButtonConfigHelper buttonHelper;

            public string startText;
            public string changeToText;
            private bool swapped = false;

            // Start is called before the first frame update
            void Awake()
            {
                buttonHelper = GetComponent<ButtonConfigHelper>();
                buttonHelper.MainLabelText = startText;
            }

            private void OnEnable()
            {
                buttonHelper.OnClick.AddListener(OnClickLabelChange);
            }

            private void OnDisable()
            {
                buttonHelper.OnClick.RemoveListener(OnClickLabelChange);
            }

            public void OnClickLabelChange()
            {
                if (!swapped)
                {
                    buttonHelper.MainLabelText = changeToText;
                    swapped = true;
                }
                else
                {
                    buttonHelper.MainLabelText = startText;
                    swapped = false;
                }
            }

            public void SetDefault()
            {
                if (swapped)
                {
                    buttonHelper.MainLabelText = startText;
                    swapped = false;
                }
            }

            public void SetSecondary()
            {
                if (!swapped)
                {
                    buttonHelper.MainLabelText = changeToText;
                    swapped = true;
                }
            }
        }
    }
}
