using ProgramAR.Sets;
using UnityEngine;

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Set/ZoneObjectRuntimeDictionary"), System.Serializable]
    public class ZoneObjectRuntimeDictionary : RuntimeDictionary<int, ZoneObject>
    {
    }
}

