

using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// </summary>
/// 


    public abstract class EventBase : ScriptableObject
    {
    }
    public abstract class EventBase<T> : EventBase, IClearListeners
    {

        public List<T> listeners = new List<T>();

        public virtual void ClearListeners()
        {
            listeners.Clear();
        }

        public virtual void RegisterListener(T listener) => listeners.Add(listener);

        public virtual void UnregisterListener(T listener) => listeners.Remove(listener);
    }

