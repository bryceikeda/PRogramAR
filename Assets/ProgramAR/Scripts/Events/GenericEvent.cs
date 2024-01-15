using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Events
{
    [CreateAssetMenu(menuName = "Event/GenericEvent"), System.Serializable]
    public class GenericEvent : ScriptableObject, IClearListeners
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        private List<EventListenerBase> listeners = new List<EventListenerBase>();

        public virtual void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        public virtual void RegisterListener(EventListenerBase listener) => listeners.Add(listener);

        public virtual void UnregisterListener(EventListenerBase listener) => listeners.Remove(listener);

        public void ClearListeners()
        {
            listeners.Clear();
        }

    }
}