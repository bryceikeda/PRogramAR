using UnityEngine;
using ProgramAR.Pages;

namespace ProgramAR
{
    namespace Pages
    {
        [CreateAssetMenu(menuName = "Factory/BoxObjectFactory"), System.Serializable]
        public class BoxObjectFactory : ScriptableObject
        {
            [SerializeField]
            BoxObject prefab;

            public BoxObject GetInstance()
            {
                return Instantiate(prefab);
            }
        }
    }
}










