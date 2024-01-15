 

using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/SaveTAPEvent"), System.Serializable]
    public class SaveTAPEvent : EventBase<ISaveTAPResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnSaveTAPEvent();
        }
    }
}
