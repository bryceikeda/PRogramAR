using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using ProgramAR.Events;
using ProgramAR.Variables; 

namespace ProgramAR.ButtonHelpers
{
    public class DeleteButton : EventListenerBase
    {
        [SerializeField] BoolVariable isEnabledBool;

        public override void OnEventRaised()
        {
            GetComponent<Interactable>().enabled = isEnabledBool.Value;
            GetComponent<PressableButtonHoloLens2>().enabled = isEnabledBool.Value;
            // Hide/show back plate, text and icon
            gameObject.transform.GetChild(3).gameObject.SetActive(isEnabledBool.Value);
            gameObject.transform.GetChild(4).gameObject.SetActive(isEnabledBool.Value);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnEventRaised();
        }
    }
}