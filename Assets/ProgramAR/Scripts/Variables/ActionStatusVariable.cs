using RosMessageTypes.Moveit;
using UnityEngine;

namespace ProgramAR.Variables
{

    [CreateAssetMenu(menuName = "Variable/ActionStatusVariable"), System.Serializable]
    public class ActionStatusVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public bool Done;
        public MoveItErrorCodesMsg ErrorCode; 

        public void SetValues(bool value, MoveItErrorCodesMsg msg)
        {
            Done = value;
            ErrorCode = msg; 
        }

        public void SetValue(BoolVariable value)
        {
            Done = value.Value;
        }

        public void FlipBool()
        {
            Done = !Done;
        }
    }
}