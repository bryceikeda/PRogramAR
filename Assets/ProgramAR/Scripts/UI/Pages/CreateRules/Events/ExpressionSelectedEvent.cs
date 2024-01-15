using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Event/ExpressionSelectedEvent"), System.Serializable]
    public class ExpressionSelectedEvent : EventBase<IExpressionSelectedResponse>
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        public void Raise(int selectedRow, Expression selectedExpression)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnExpressionSelectedEvent(selectedRow, selectedExpression);
        }
    }
}