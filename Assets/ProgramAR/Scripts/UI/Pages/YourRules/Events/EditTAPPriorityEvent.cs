using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/EditTAPPriorityEvent"), System.Serializable]
    public class EditTAPPriorityEvent : EventBase<IEditTAPPriorityResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public virtual void Raise(int initialPriority)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEditTAPPriorityEvent(initialPriority);
        }
    }
}