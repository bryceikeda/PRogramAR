using UnityEngine;

namespace ProgramAR.Events
{
    public abstract class EventListenerBase : MonoBehaviour
    {
        public GenericEvent Event;

        protected virtual void OnEnable() { Event.RegisterListener(this); }

        protected virtual void OnDisable() { Event.UnregisterListener(this); }

        public abstract void OnEventRaised();
    }
}
