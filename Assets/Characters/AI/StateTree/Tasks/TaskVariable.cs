using System;
using FoxShooter.Characters.AI.StateTree.UI;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    [Serializable]
    public abstract class TaskVariable
    {
        protected enum ContextTag : byte
        {
            Variable,
            Context,
            Blackboard
        }
        [SerializeField] protected ContextTag tag;
        
        public abstract object data { get; set; }
        public abstract Type type { get; }
    }
    
    /**
     * <summary>
     * A variable that can be found inside a task. Can either be assigned to a direct value,
     * or call a Context of matching type to get that variable.
     * </summary>
     */
    [Serializable]
    public class TaskVariable<T> : TaskVariable
    {
        [SerializeReference] protected object internalData;

        public override object data
        {
            get => internalData;
            set
            {
                internalData = tag switch
                {
                    ContextTag.Variable => (T)value,
                    ContextTag.Context => value,
                    ContextTag.Blackboard => value,
                    _ => throw new NotImplementedException()
                };
            }
        }
        
        public override Type type { get => typeof(T); }

        public void Set(T variable)
        {
            tag = ContextTag.Variable;
            data = variable;
        }
        
        public void Set(BlackboardReference<T> newReference)
        {
            tag = ContextTag.Blackboard;
            data = newReference;
        }

        public void Set(IContext<T> newContext)
        {
            tag = ContextTag.Context;
            data = newContext;
        }
        
        public T Get(TreeContext treeContext)
        {
            return tag switch
            {
                ContextTag.Variable => (T)data,
                ContextTag.Context => ((IContext<T>)data).Evaluate(treeContext),
                ContextTag.Blackboard => ((BlackboardReference<T>)data).value,
                _ => throw new ArgumentOutOfRangeException()
            };

        }
    }
}