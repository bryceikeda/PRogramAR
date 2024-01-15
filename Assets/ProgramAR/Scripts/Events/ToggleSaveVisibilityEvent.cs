using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Events
{
    [CreateAssetMenu(menuName = "Event/ToggleSaveVisibilityEvent"), System.Serializable]
    public class ToggleSaveVisibilityEvent : EventBase<IToggleSaveVisibilityResponse>
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>


        public void Raise(bool visibility)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnToggleSaveVisibilityEvent(visibility);
        }

        public void Raise(bool visibility, string saveOrigin)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnToggleSaveVisibilityEvent(visibility, saveOrigin);
        }
    }
}