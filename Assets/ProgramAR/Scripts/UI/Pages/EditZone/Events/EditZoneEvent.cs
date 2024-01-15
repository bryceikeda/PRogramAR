using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/EditZoneEvent"), System.Serializable]
    public class EditZoneEvent : EventBase<IEditZoneResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif


        public void Raise(int selectedZone)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEditZoneEvent(selectedZone);
        }
    }
}