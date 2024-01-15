using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/UpdatePlanningSceneEvent"), System.Serializable]
    public class UpdatePlanningSceneEvent : EventBase<IUpdatePlanningSceneResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public void Raise(string movingObject)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnUpdatePlanningSceneEvent(movingObject);
        }
    }
}
