using UnityEngine;
using ProgramAR.Events;

namespace RuleProcessor
{
    [CreateAssetMenu(menuName = "Event/UpdateRunStatusEvent"), System.Serializable]
    public class UpdateRunStatusEvent : EventBase<IUpdateRunStatusResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise(string status)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnUpdateRunStatusEvent(status);
        }
    }
}
