using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Variables
{
    public class ResetBoolVariables : MonoBehaviour
    {
        [SerializeField] List<BoolVariable> variables;
        public bool defaultValue = false;

        private void OnApplicationQuit()
        {
            foreach (BoolVariable variable in variables)
            {
                variable.SetValue(false);
            }
        }
    }
}