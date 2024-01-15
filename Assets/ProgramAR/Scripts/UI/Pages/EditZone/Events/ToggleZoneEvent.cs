using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/ToggleZoneEvent"), System.Serializable]
    public class ToggleZoneEvent : EventBase<IToggleZoneResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise(int selectedZone, bool isToggled)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnToggleZoneEvent(selectedZone, isToggled);
        }
    }

}
