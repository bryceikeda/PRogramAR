 

using UnityEngine;

namespace ProgramAR
{
    namespace Pages
    {
        [CreateAssetMenu(menuName = "Factory/ZoneButtonGroupFactory"), System.Serializable]
        public class ZoneButtonGroupFactory : ScriptableObject
        {
            [SerializeField]
            ZoneButtonGroup prefab;

            public ZoneButtonGroup GetInstance()
            {
                return Instantiate(prefab);
            }
        }
    }
}










