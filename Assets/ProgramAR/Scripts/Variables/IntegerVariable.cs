using UnityEngine;

namespace ProgramAR.Variables
{

    [CreateAssetMenu(menuName = "Variable/IntegerVariable"), System.Serializable]
    public class IntegerVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public int Value;

        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetValue(IntegerVariable value)
        {
            Value = value.Value;
        }
    }
}