 

using UnityEngine;

namespace ProgramAR
{
    namespace Pages
    {
        [CreateAssetMenu(menuName = "Factory/ClauseButtonGroupFactory"), System.Serializable]
        public class ClauseButtonGroupFactory : ScriptableObject
        {
            [SerializeField]
            ClauseButtonGroup prefab;

            public ClauseButtonGroup GetInstance()
            {
                return Instantiate(prefab);
            }
        }
    }
}










