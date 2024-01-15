 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class ResetClauseRuntimeSet : MonoBehaviour
    {
        [SerializeField] List<ClauseRuntimeSet> sets;

        private void OnApplicationQuit()
        {
            foreach (ClauseRuntimeSet set in sets)
            {
                set.Items.Clear(); 
            }
        }
    }
}
