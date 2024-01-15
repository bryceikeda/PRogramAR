
using ProgramAR.Sets;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Set/BoxObjectRuntimeDictionary"), System.Serializable]
    public class BoxObjectRuntimeDictionary : RuntimeDictionary<string, BoxObject>
    {
    }
}

