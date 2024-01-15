using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/ShowClauseListEvent"), System.Serializable]
    public class ShowClauseListEvent : EventBase<IShowClauseListResponse>
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>


        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnShowClauseListEvent();
        }
    }
}