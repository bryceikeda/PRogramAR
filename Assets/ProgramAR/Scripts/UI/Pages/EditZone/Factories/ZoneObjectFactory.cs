 

using UnityEngine;

namespace ProgramAR
{
    namespace Pages
    {
        [CreateAssetMenu(menuName = "Factory/ZoneObjectFactory"), System.Serializable]
        public class ZoneObjectFactory : ScriptableObject
        {
            [SerializeField]
            ZoneObject prefab;

            public ZoneObject GetInstance()
            {
                return Instantiate(prefab);
            }
        }
    }
}










