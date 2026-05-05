using System;
using System.Collections.Generic;

namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    [Serializable]
    public abstract class Task
    {
        private List<TaskVariable> _taskVariables;

        protected Task()
        {
            _taskVariables = new List<TaskVariable>();
            
            foreach (var fieldInfo in GetType().GetFields())
            {
                if (fieldInfo.FieldType != typeof(TaskVariable))
                {
                    continue;
                }
                
                _taskVariables.Add(fieldInfo.GetValue(this) as TaskVariable);
            }
        }
        
        public abstract TaskStatus Enter();

        public abstract TaskStatus Update(TreeContext context, float time);

        public virtual void Exit() {}

        public List<TaskVariable> GetVariables()
        {
            var result = new List<TaskVariable>();
            foreach (var fieldInfo in GetType().GetFields())
            {
                if (fieldInfo.FieldType == typeof(TaskVariable))
                {
                    result.Add((TaskVariable)fieldInfo.GetValue(this));
                }
            }
            return result;
        }
    }
}