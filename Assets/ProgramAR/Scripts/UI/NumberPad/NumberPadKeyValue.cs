using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

namespace ProgramAR
{
    namespace NumberPad
    {
        /// <summary>
        /// Represents a key on the keyboard that has a string value for input.
        /// </summary>
        [RequireComponent(typeof(Interactable))]
        public class NumberPadKeyValue : MonoBehaviour
        {
            /// <summary>
            /// The default string value for this key.
            /// </summary>
            public string Value;

            /// <summary>
            /// Reference to child text element.
            /// </summary>
            private TextMeshPro m_Text;

            /// <summary>
            /// Reference to the GameObject's button component.
            /// </summary>
            private Interactable m_Button;

            /// <summary>
            /// Initialize key text, subscribe to the onClick event, and subscribe to keyboard shift event.
            /// </summary>
            private void Start()
            {
                m_Button = GetComponent<Interactable>();
                m_Text = gameObject.GetComponentInChildren<TextMeshPro>();
                m_Text.text = Value;

                m_Button.OnClick.RemoveAllListeners();
                m_Button.OnClick.AddListener(FireAppendValue);
            }

            /// <summary>
            /// Method injected into the button's onClick listener.
            /// </summary>
            private void FireAppendValue()
            {
                NumberPadHandler.Instance.AppendValue(this);
            }
        }
    }
}


