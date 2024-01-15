using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

namespace ProgramAR
{
    namespace ButtonHelpers
    {
        [RequireComponent(typeof(ButtonConfigHelper))]
        public class ButtonSecondaryLabelToggle : MonoBehaviour
        {
            ButtonConfigHelper buttonHelper;
            public TextMeshPro label;

            public string startText;
            public string changeToText;
            private bool swapped = false;

            // Start is called before the first frame update
            void Start()
            {
                buttonHelper = GetComponent<ButtonConfigHelper>();
                buttonHelper.OnClick.AddListener(() => { OnClickLabelChange(); });
                label.text = startText;
            }

            public void OnClickLabelChange()
            {
                if (!swapped)
                {
                    label.text = changeToText;
                    swapped = true;
                }
                else
                {
                    label.text = startText;
                    swapped = false;
                }
            }
        }
    }
}