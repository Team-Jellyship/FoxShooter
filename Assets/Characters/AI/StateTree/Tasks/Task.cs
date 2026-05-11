using System;
using System.Collections.Generic;

namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    [Serializable]
    public abstract class Task
    {
        public IEnumerable<TaskVariable> taskVariables { get => _taskVariables; }
        
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

        public List<Tuple<string, TaskVariable>> GetClassVariables()
        {
            var result = new List<Tuple<string, TaskVariable>>();
            foreach (var fieldInfo in GetType().GetFields())
            {
                if (!fieldInfo.FieldType.IsSubclassOf(typeof(TaskVariable)))
                {
                    continue;
                }
                
                var pair = new Tuple<string, TaskVariable>(fieldInfo.Name, (TaskVariable)fieldInfo.GetValue(this));
                result.Add(pair);
            }
            return result;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}