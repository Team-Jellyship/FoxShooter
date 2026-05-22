using System;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree
{
    [Serializable]
    public abstract class BlackboardReference
    {
        [SerializeField] public string name;

        public abstract void InitializeReference(Blackboard blackboard);
    }
    
    [Serializable]
    public class BlackboardReference<T> : BlackboardReference
    {
        public BlackboardReference(string key)
        {
            name = key;
        }
        
        public BlackboardReference(Blackboard blackboard, string key)
        {
            name = key;
            InitializeReference(blackboard);
        }
        
        private WeakReference<BlackboardVariable<T>> _valueReference;

        public T value
        {
            get
            {
                _valueReference.TryGetTarget(out var reference);
                return reference == null ? default : reference.value;
            }
        }

        public override sealed void InitializeReference(Blackboard blackboard)
        {
            if (!blackboard.TryGetVariable<T>(name, out var data))
            {
                Debug.LogError($"[BlackboardReference] Couldn't find variable with key '{name}', the reference was missing");
            }
            _valueReference = new WeakReference<BlackboardVariable<T>>(data);
        }
    }
}