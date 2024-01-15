 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class ResetTriggerActionRules : MonoBehaviour
    {
        [SerializeField] TriggerActionRules tap;

        private void Start()
        {
            
        }

        private void OnApplicationQuit()
        {
            tap.pairs.Clear(); 
        }
    }
}
