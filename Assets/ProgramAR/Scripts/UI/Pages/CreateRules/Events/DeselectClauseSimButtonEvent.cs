using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/DeselectClauseSimButtonEvent"), System.Serializable]
    public class DeselectClauseSimButtonEvent : EventBase<IDeselectClauseSimButtonEvent>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public virtual void Raise(int selectedClause, string description)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnDeselectClauseSimButtonEvent(selectedClause, description);
        }
    }
}