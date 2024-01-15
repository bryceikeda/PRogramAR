using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/EditTAPEvent"), System.Serializable]
    public class EditTAPEvent : EventBase<IEditTAPResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise(int selectedTAP)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEditTAPEvent(selectedTAP);
        }
    }
}