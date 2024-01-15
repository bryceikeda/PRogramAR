using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class ClearListeners : MonoBehaviour
    {
        public List<EventBase> eventListeners; 

        public bool clear = true;
        private void OnApplicationQuit()
        {
            if (clear)
            {
                for (int i = 0; i < eventListeners.Count; i++)
                {
                    IClearListeners toClear = (IClearListeners)eventListeners[i];
                    toClear.ClearListeners();
                }
            }
        }
    }

}
