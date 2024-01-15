using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.NumberPad
{
    [CreateAssetMenu(menuName = "Event/UpdatePriorityEvent"), System.Serializable]
    public class UpdatePriorityEvent : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        private List<IUpdatePriorityResponse> listeners = new List<IUpdatePriorityResponse>();

        public virtual void Raise(int newPriority)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnUpdatePriorityEvent(newPriority);
        }

        public virtual void RegisterListener(IUpdatePriorityResponse listener) => listeners.Add(listener);

        public virtual void UnregisterListener(IUpdatePriorityResponse listener) => listeners.Remove(listener);
    }

}
