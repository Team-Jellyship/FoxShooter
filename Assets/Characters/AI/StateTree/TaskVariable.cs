using System;
using FoxShooter.Characters.AI.StateTree.UI;

namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    /**
     * <summary>
     * A variable that can be found inside a task. Can either be assigned to a direct value,
     * or call a Context of matching type to get that variable.
     * </summary>
     */
    public class TaskVariable<T>
    {
        private enum ContextTag : byte
        {
            Variable,
            Context,
            Blackboard
        }

        public void Set(T variable)
        {
            _tag = ContextTag.Variable;
            _context = null;
            _blackboardReference = null;
            _variable = variable;
        }
        
        public void Set(BlackboardReference<T> newReference)
        {
            _tag = ContextTag.Blackboard;
            _context = null;
            _variable = default;
            _blackboardReference = newReference;
        }

        public void Set(IContext<T> newContext)
        {
            _tag = ContextTag.Context;
            _variable = default;
            _blackboardReference = null;
            _context = newContext;
        }
        
        public T Get(TreeContext treeContext)
        {
            return _tag switch
            {
                ContextTag.Variable => _variable,
                ContextTag.Context => _context.Evaluate(treeContext),
                ContextTag.Blackboard => _blackboardReference.value,
                _ => throw new ArgumentOutOfRangeException()
            };

        }

        private ContextTag _tag;
        private T _variable;
        private IContext<T> _context;
        private BlackboardReference<T> _blackboardReference;
    }
}