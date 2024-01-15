using UnityEngine;

namespace ProgramAR.Pages
{
    public class ZoneButtonGroup : MonoBehaviour
    { 
        public ToggleZoneButton toggleZoneButton;
        public EditZoneButton editZoneButton;
        public DeleteZoneButton deleteZoneButton;

        public void SetZoneButtonGroup(int selectedZone, string label, Color color)
        {
            toggleZoneButton.SetProperties(selectedZone);
            editZoneButton.SetProperties(selectedZone, label, color);
            deleteZoneButton.SetProperties(selectedZone);
        }
    }
}
