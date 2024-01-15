 
using UnityEngine;

namespace ProgramAR.Variables
{
    [CreateAssetMenu(menuName = "Variable/FloatVariable"), System.Serializable]
    public class FloatVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public float Value;

        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }
    }
}