using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace ProgramAR
{
    namespace NumberPad
    {
        /// <summary>
        /// Represents a key on the keyboard that has a function.
        /// </summary>
        [RequireComponent(typeof(Interactable))]
        public class NumberPadKeyFunc : MonoBehaviour
        {
            /// <summary>
            /// Possible functionality for a button.
            /// </summary>
            public enum Function
            {
                // Commands
                Enter,
                Close,
                // Editing
                Backspace,
                UNDEFINED
            }

            /// <summary>
            /// Designer specified functionality of a keyboard button.
            /// </summary>
            [SerializeField]
            private Function buttonFunction = Function.UNDEFINED;

            public Function ButtonFunction => buttonFunction;

            /// <summary>
            /// Subscribe to the onClick event.
            /// </summary>
            private void Start()
            {
                Interactable m_Button = GetComponent<Interactable>();
                m_Button.OnClick.RemoveAllListeners();
                m_Button.OnClick.AddListener(FireFunctionKey);
            }

            /// <summary>
            /// Method injected into the button's onClick listener.
            /// </summary>
            private void FireFunctionKey()
            {
                NumberPadHandler.Instance.FunctionKey(this);
            }
        }
    }
}