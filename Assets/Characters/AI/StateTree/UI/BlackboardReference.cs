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
            blackboard.TryGetVariable<T>(key, out var data);
            _valueReference = new WeakReference<BlackboardVariable<T>>(data);
        }
        
        [SerializeField] private string name;

        private WeakReference<BlackboardVariable<T>> _valueReference;

        public T value
        {
            get
            {
                _valueReference.TryGetTarget(out var reference);
                return reference == null ? default : reference.value;
            }
        }
    }
}