using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/DeleteClauseEvent"), System.Serializable]
    public class DeleteClauseEvent : EventBase<IDeleteClauseResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise(int clauseIndex)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnDeleteClauseEvent(clauseIndex);
        }
    }

}
