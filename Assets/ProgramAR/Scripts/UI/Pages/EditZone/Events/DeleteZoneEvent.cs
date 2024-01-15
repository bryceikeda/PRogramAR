using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/DeleteZoneEvent"), System.Serializable]
    public class DeleteZoneEvent : EventBase<IDeleteZoneResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public void Raise(int selectedZone)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnDeleteZoneEvent(selectedZone);
        }
    }

}
