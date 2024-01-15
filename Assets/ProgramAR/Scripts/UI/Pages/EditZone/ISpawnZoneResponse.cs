using UnityEngine;

namespace ProgramAR.Pages
{
    public interface ISpawnZoneResponse
    {
        void OnSpawnZoneEvent(int selectedZone, SerializedTransform t = null); 
    }
}
