using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    public class ResetZoneObjectRuntimeDictionary : MonoBehaviour
    {
        [SerializeField] ZoneObjectRuntimeDictionary zoneObjectRuntimeDictionary;

        private void OnApplicationQuit()
        {
            zoneObjectRuntimeDictionary.Items.Clear(); 
        }
    }
}
