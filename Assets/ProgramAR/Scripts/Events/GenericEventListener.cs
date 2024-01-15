using UnityEngine.Events;

namespace ProgramAR.Events
{
    public class GenericEventListener : EventListenerBase
    {
        public UnityEvent Response;

        public override void OnEventRaised() { Response.Invoke(); }
    }

}
