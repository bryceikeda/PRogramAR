using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    [System.Serializable]
    public class TriggerActionPair
    {
        public List<Clause> trigger;
        public List<Clause> action;
    }
}
