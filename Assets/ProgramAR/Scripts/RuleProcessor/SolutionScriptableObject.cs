 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.MoveitTaskConstructor;

/// <summary>
///
/// </summary>

namespace RuleProcessor
{
    [CreateAssetMenu(fileName = "SolutionScriptableObject", menuName = "SolutionScriptableObject")]
    public class SolutionScriptableObject : ScriptableObject
    {
        public List<SolutionMsg> solution; 
    }
}
