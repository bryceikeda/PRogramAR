using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/DuplicateTAPEvent"), System.Serializable]
    public class DuplicateTAPEvent : EventBase<IDuplicateTAPResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise(int selectedTAP)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnDuplicateTAPEvent(selectedTAP);
        }
    }
}