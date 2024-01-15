using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Set/ExpressionTree"), System.Serializable]
    public class ExpressionTree : ScriptableObject
    {
        [System.Serializable]
        public class ExpressionNode
        {
            public string expressionNodeName;
            public List<string> expressionNames;
            public List<string> nextExpressionNodes;
            public List<string> expressionIconName;
        }

        public ExpressionNode[] expressionNodes;
    }
}