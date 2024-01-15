 
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class DeleteZoneButton : MonoBehaviour
    {
        [SerializeField] Interactable interactable;
        [SerializeField] DeleteZoneEvent deleteZoneEvent;
        private int selectedZone;

        public void SetProperties(int selectedZone)
        {
            this.selectedZone = selectedZone;
        }

        public void DeleteZone()
        {
            deleteZoneEvent.Raise(selectedZone);
        }

        private void OnEnable()
        {
            interactable.OnClick.AddListener(DeleteZone);
        }

        private void OnDisable()
        {
            interactable.OnClick.RemoveListener(DeleteZone);
        }

    }

}
