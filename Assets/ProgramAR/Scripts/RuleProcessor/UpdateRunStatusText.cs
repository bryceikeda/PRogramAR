using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;

namespace RuleProcessor
{
    public class UpdateRunStatusText : MonoBehaviour, IUpdateRunStatusResponse
    {
        [SerializeField] UpdateRunStatusEvent updateRunStatusEvent;
        [SerializeField] Interactable interactable; 
        [SerializeField] TextMeshPro text; 
        public void OnUpdateRunStatusEvent(string status)
        {
            text.text = status; 
        }

        public void UpdateDefaultText()
        {
            if (!interactable.IsToggled)
            {
                text.text = "ProgramAR";
            }
        }

        private void OnEnable()
        {
            interactable.OnClick.AddListener(UpdateDefaultText);
            updateRunStatusEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            interactable.OnClick.RemoveListener(UpdateDefaultText);
            updateRunStatusEvent.UnregisterListener(this);
        }


    }
}
