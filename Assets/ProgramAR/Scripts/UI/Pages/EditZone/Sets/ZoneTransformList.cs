using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>

namespace ProgramAR.Pages
{
    [CreateAssetMenu(menuName = "Set/ZoneTransformList"), System.Serializable]
    public class ZoneTransformList : ScriptableObject
    {
        public List<SerializedTransform> transforms;
        public List<int> indexes; 

        public void Add(int index, SerializedTransform trans)
        {
            indexes.Add(index);
            transforms.Add(trans);
        }

        public void Clear()
        {
            transforms.Clear();
            indexes.Clear(); 
        }
    }
}
