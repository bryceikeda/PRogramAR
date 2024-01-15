 
using UnityEngine;

namespace ProgramAR.Variables
{
    [CreateAssetMenu(menuName = "Variable/FloatRangeVariable"), System.Serializable]
    public class FloatRangeVariable : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        [Range(0f, 1.0f)]
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