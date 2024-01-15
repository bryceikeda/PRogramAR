using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuleProcessor
{
    public interface IPlanStatusResponse
    {
        void OnPlanStatusEvent(string status); 
    }
}
