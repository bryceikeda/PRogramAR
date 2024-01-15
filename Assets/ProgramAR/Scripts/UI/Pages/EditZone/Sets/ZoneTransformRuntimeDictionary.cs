using ProgramAR.Sets;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Set/ZoneTransformRuntimeDictionary"), System.Serializable]
    public class ZoneTransformRuntimeDictionary : RuntimeDictionary<int, SerializedTransform>
    {
    }
}
