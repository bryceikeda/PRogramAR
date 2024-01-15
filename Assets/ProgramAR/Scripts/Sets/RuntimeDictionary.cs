using System.Collections.Generic;
using UnityEngine;

namespace ProgramAR.Sets
{
    [System.Serializable]
    public abstract class RuntimeDictionary<T1, T2> : ScriptableObject
    {
        public Dictionary<T1, T2> Items = new Dictionary<T1, T2>();

        public void Add(T1 key, T2 value)
        {
            if (!Items.TryGetValue(key, out T2 val))
                Items.Add(key, value);
        }

        public void Remove(T1 key)
        {
            if (!Items.TryGetValue(key, out T2 val))
                Items.Remove(key);
        }
    }
}