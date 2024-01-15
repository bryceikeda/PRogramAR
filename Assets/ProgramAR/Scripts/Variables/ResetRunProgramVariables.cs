using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Variables
{
    public class ResetRunProgramVariables : MonoBehaviour
    {
        [SerializeField] ActionStatusVariable actionStatus;
        [SerializeField] BoolVariable executeCommand;

        public void Reset()
        {
            actionStatus.Done = true;
            executeCommand.Value = false; 
        }

        private void OnApplicationQuit()
        {
            Reset(); 
        }
    }

}
