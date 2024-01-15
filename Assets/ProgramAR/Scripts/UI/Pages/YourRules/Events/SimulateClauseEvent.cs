using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/SimulateClauseEvent"), System.Serializable]
    public class SimulateClauseEvent : EventBase<ISimulateClauseResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public virtual void Raise(int selectedClause, bool simulate)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnSimulateClauseEvent(selectedClause, simulate);
        }
    }
}