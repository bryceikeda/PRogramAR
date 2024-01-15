using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/EditClauseEvent"), System.Serializable]
    public class EditClauseEvent : EventBase<IEditClauseEventResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public virtual void Raise(int selectedClause)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEditClauseEvent(selectedClause);
        }
    }
}