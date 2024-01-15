using UnityEngine;

namespace ProgramAR
{
    namespace Pages
    {
        [CreateAssetMenu(menuName = "Factory/TAPButtonGroupFactory"), System.Serializable]
        public class TAPButtonGroupFactory : ScriptableObject
        {
            [SerializeField]
            TAPButtonGroup prefab;

            public TAPButtonGroup GetInstance()
            {
                return Instantiate(prefab);
            }
        }
    }
}










