using System;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree
{
    [Serializable]
    public abstract class BlackboardVariable
    {
        [SerializeField] public string name;
        
        public abstract object objectData { get; set; }

        public abstract Type type { get; }

        public override string ToString()
        {
            return name;
        }
    }

    [Serializable]
    public class BlackboardVariable<T> : BlackboardVariable
    {
        [SerializeField] protected T internalValue;

        public BlackboardVariable(string name)
        {
            this.name = name;
        }
        
        public BlackboardVariable(string name, T defaultValue = default)
        {
            this.name = name;
            internalValue = defaultValue;
        }
        
        public override object objectData
        {
            get => internalValue;
            set => internalValue = (T) value;
        }

        public T value
        {
            get => internalValue;
        }

        public override Type type
        {
            get => typeof(T);
        }
    }
}