using System;
using TMPro;
using UnityEngine;

namespace ProgramAR
{
    namespace NumberPad
    {
        public class NumberPadHandler : MonoBehaviour
        {
            public static NumberPadHandler Instance { get; private set; }

            /// <summary>
            /// The InputField that the keyboard uses to show the currently edited text.
            /// If you are using the Keyboard prefab you can ignore this field as it will
            /// be already assigned.
            /// </summary>
            public TextMeshPro InputField = null;

            public string placeHolderText = "Enter Priority...";
            public string highIndexError = "Error: Priority too high";

            [SerializeField] UpdatePriorityEvent updatePriorityEvent;
            [SerializeField] GameObject tapList; 

            /// <summary>
            /// Deactivate on Awake.
            /// </summary>
            void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(this);
                }
                else
                {
                    Instance = this;
                }
                InputField.text = placeHolderText;
            }

            void OnEnable()
            {
                InputField.text = placeHolderText;
            }

            public void AppendValue(NumberPadKeyValue value)
            {
                if (InputField.text.Equals(placeHolderText) || InputField.text.Equals(highIndexError))
                {
                    InputField.text = "";
                }
                if (InputField.text.Length < 2 && !(InputField.text.Length == 0 && value.Value.Equals("0")))
                {
                    InputField.text += value.Value;
                }
            }

            public void FunctionKey(NumberPadKeyFunc functionKey)
            {
                switch (functionKey.ButtonFunction)
                {
                    case NumberPadKeyFunc.Function.Enter:
                        {
                            Enter();
                            break;
                        }
                    case NumberPadKeyFunc.Function.Backspace:
                        {
                            Backspace();
                            break;
                        }
                    case NumberPadKeyFunc.Function.Close:
                        {
                            Close();
                            break;
                        }
                    case NumberPadKeyFunc.Function.UNDEFINED:
                        {
                            Debug.LogErrorFormat("The {0} key on this keyboard hasn't been assigned a function.", functionKey.name);
                            break;
                        }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private void Enter()
            {
                if (!InputField.text.Equals(""))
                {
                    if (Int32.Parse(InputField.text) > (tapList.transform.childCount - 1))
                    {
                        InputField.text = highIndexError;
                    }
                    else if (!InputField.text.Equals(placeHolderText) || !InputField.text.Equals(highIndexError))
                    {
                        updatePriorityEvent.Raise(Int32.Parse(InputField.text) - 1);
                    }
                }
            }

            private void Backspace()
            {
                string inputField = InputField.text;
                if (inputField.Equals(placeHolderText) || inputField.Equals(highIndexError))
                {
                    InputField.text = ""; 
                }
                else if (inputField.Length > 0)
                {
                    InputField.text = inputField.Remove(inputField.Length - 1);
                }
            }

            public void Close()
            {
                updatePriorityEvent.Raise(-1);
            }
        }
    }
}

