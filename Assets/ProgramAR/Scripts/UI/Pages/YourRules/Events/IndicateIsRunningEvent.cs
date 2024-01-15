using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/IndicateIsRunningEvent"), System.Serializable]
    public class IndicateIsRunningEvent : EventBase<IIndicateIsRunningResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public virtual void Raise(int index, bool isRunning)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnIndicateIsRunningEvent(index, isRunning);
        }
    }
}