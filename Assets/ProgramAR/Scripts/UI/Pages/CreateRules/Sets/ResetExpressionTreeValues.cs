 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProgramAR.Pages.ExpressionTree;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class ResetExpressionTreeValues : MonoBehaviour
    {
        [SerializeField] List<ExpressionTree> trees;
        [SerializeField] List<string> nodeNames; 

        private void OnApplicationQuit()
        {
            foreach (ExpressionTree tree in trees)
            {
                foreach (ExpressionNode node in tree.expressionNodes)
                {
                    foreach (string nodeName in nodeNames)
                    {
                        if (node.expressionNodeName.Equals(nodeName))
                        {
                            node.expressionNames.Clear();
                            node.nextExpressionNodes.Clear();
                            node.expressionIconName.Clear(); 
                        }
                    }
                }
            }
        }
    }
}
