using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/SelectRuleTypeEvent"), System.Serializable]
    public class SelectRuleTypeEvent : EventBase<ISelectRuleTypeResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public void Raise(string type)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnSelectRuleTypeEvent(type);
        }
    }

}
