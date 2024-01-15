using UnityEngine;
using ProgramAR.Events;

namespace RuleProcessor
{
    [CreateAssetMenu(menuName = "Event/ActionCompleteEvent"), System.Serializable]
    public class ActionCompleteEvent : EventBase<IActionCompleteResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnActionCompleteEvent();
        }
    }
}
