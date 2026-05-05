using System;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    [Serializable]
    public class BlackboardReference<T>
    {
        public BlackboardReference(Blackboard blackboard, string key)
        {
            name = key;
            if (!blackboard.TryGetVariable<T>(key, out var data))
            {
                Debug.LogError($"[BlackboardReference] Couldn't find variable with key '{key}', the reference was missing");
            }
            _valueReference = new WeakReference<BlackboardVariable<T>>(data);
        }
        
        [SerializeField] private string name;

        private WeakReference<BlackboardVariable<T>> _valueReference;

        public T value
        {
            get
            {
                _valueReference.TryGetTarget(out var reference);
                if (reference == null)
                {
                    Debug.Log("NULLREFERENCE!!!!");
                }
                return reference == null ? default : reference.value;
            }
        }
    }
}