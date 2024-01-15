using UnityEngine;

namespace ProgramAR.Variables
{

    [CreateAssetMenu(menuName = "Variable/BoolVariable"), System.Serializable]
    public class BoolVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public bool Value;

        public void SetValue(bool value)
        {
            Value = value;
        }

        public void SetValue(BoolVariable value)
        {
            Value = value.Value;
        }

        public void FlipValue()
        {
            Value = !Value;
        }
    }
}