using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Events
{
    [CreateAssetMenu(menuName = "Event/BackEvent"), System.Serializable]
    public class BackEvent : EventBase<IBackResponse>
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnBackEvent();
        }
    }
}