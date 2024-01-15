using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/SpawnZoneEvent"), System.Serializable]
    public class SpawnZoneEvent : EventBase<ISpawnZoneResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public void Raise(int selectedZone, SerializedTransform t = null)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnSpawnZoneEvent(selectedZone, t);
        }
    }

}
