 

using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class ToggleZoneButton : MonoBehaviour
    {
        [SerializeField] ToggleZoneEvent Event; 
        [SerializeField] Interactable interactable;
        private int selectedZone;
        public void SetProperties(int selectedZone)
        {
            this.selectedZone = selectedZone;
        }

        public void ToggleZone()
        {
            Event.Raise(selectedZone, interactable.IsToggled);
        }

        public void AddListener(UnityAction action)
        {
            interactable.OnClick.AddListener(action); 
        }

        public void RemoveListener(UnityAction action)
        {
            interactable.OnClick.RemoveListener(action); 
        }

        private void OnEnable()
        {
            interactable.OnClick.AddListener(ToggleZone);
        }

        private void OnDisable()
        {
            interactable.OnClick.RemoveListener(ToggleZone);
        }
    }
}
