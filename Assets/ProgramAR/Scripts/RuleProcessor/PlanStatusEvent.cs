using UnityEngine;
using ProgramAR.Events;

namespace RuleProcessor
{
    [CreateAssetMenu(menuName = "Event/PlanStatusEvent"), System.Serializable]
    public class PlanStatusEvent : EventBase<IPlanStatusResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise(string status)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnPlanStatusEvent(status);
        }
    }
}
