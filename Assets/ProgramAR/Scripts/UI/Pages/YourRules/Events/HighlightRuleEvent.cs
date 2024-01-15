using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/HighlightRuleEvent"), System.Serializable]
    public class HighlightRuleEvent : EventBase<IHighlightRuleResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public virtual void Raise(int index, bool triggerIsValid, bool actionIsValid)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnHighlightRuleEvent(index, triggerIsValid, actionIsValid);
        }
    }
}