 
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class EditZoneButton : MonoBehaviour, IToggleZoneResponse
    {
        [SerializeField] TextMeshPro buttonTextReference;
        [SerializeField] Renderer backPlateToggleState;
        [SerializeField] Renderer backPlate;
        [SerializeField] EditZoneEvent editZoneEvent;
        [SerializeField] ToggleZoneEvent toggleZoneEvent; 
        [SerializeField] Interactable interactable;
        private int selectedZone; 

        public void SetProperties(int selectedZone, string label, Color color)
        {
            backPlateToggleState.material.color = color;
            backPlate.material.color = color;
            buttonTextReference.text = label;
            this.selectedZone = selectedZone;
            GetComponent<ButtonLabelToggle>().SetSecondary(); // Quick hack, should fix this at some point
        }

        public void OnToggleZoneEvent(int selectedZone, bool isToggled)
        {
            if (selectedZone == this.selectedZone)
            {
                if (!isToggled)
                {
                    DeSelect();
                    SetInteractionState(false);
                }
                else
                {
                    SetInteractionState(true);
                }
            }
        }

        private void SetInteractionState(bool activate)
        {
            GetComponent<PressableButtonHoloLens2>().enabled = activate;
            // Hide/show back plate, text and icon
            gameObject.transform.GetChild(3).gameObject.SetActive(activate);
            interactable.enabled = activate;
        }

        private void DeSelect()
        {
            interactable.IsToggled = false;
            transform.GetChild(5).gameObject.SetActive(false);
            GetComponent<ButtonLabelToggle>().SetDefault();
        }

        public void EditZone()
        {
            editZoneEvent.Raise(selectedZone); 
        }

        private void OnEnable()
        {
            toggleZoneEvent.RegisterListener(this);
            interactable.OnClick.AddListener(EditZone);
        }

        private void OnDisable()
        {
            toggleZoneEvent.UnregisterListener(this);
            interactable.OnClick.RemoveListener(EditZone);
        }

    }
}
