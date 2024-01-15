using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/HideTAPListEvent"), System.Serializable]
    public class HideTAPListEvent : EventBase<IHideTAPListResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnHideTAPListEvent();
        }
    }

}
