using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuleProcessor
{
    public interface IUpdateRunStatusResponse
    {
        void OnUpdateRunStatusEvent(string status); 
    }
}
