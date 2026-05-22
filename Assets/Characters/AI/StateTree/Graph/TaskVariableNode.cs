using System;
using System.Reflection;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    [Serializable]
    public abstract class TaskVariableNode
    {
        [SerializeField] public string name;
        [SerializeField] public string type;
        
        public abstract object data { get; set; }

        public abstract TaskVariable GenerateVariable();
    }

    [Serializable]
    public class TaskVariableBlackboardReference<T> : TaskVariableNode
    {
        [SerializeField] public string blackboardVariable;
        
        public override object data { get; set; }

        public static TaskVariableBlackboardReference<T> Serialize(string name, TaskVariable<T> taskVariable)
        {
            return new TaskVariableBlackboardReference<T>
            {
                name = name,
                type = taskVariable.GetType().FullName,
                blackboardVariable = ((BlackboardReference)taskVariable.data).name
            };
        }
        
        public override TaskVariable GenerateVariable()
        {
            var taskVariableType = Type.GetType(type);
            if (taskVariableType == null)
            {
                return null;
            }

            var taskVariable = (TaskVariable<T>)Activator.CreateInstance(taskVariableType);
            taskVariable.Set(new BlackboardReference<T>(blackboardVariable));
            return taskVariable;
        }
    }

    [Serializable]
    public class TaskVariableNode<T> : TaskVariableNode
    {
        [SerializeField] private T internalData;

        public override object data
        {
            get => internalData;
            set => internalData = (T) value;
        }

        public static TaskVariableNode<T> Serialize(string name, TaskVariable<T> taskVariable)
        {
            return new TaskVariableNode<T>
            {
                name = name,
                type = taskVariable.GetType().FullName,
                internalData = (T)taskVariable.data,
            };
        }
        
        public override TaskVariable GenerateVariable()
        {
            var taskVariableType = Type.GetType(type);
            if (taskVariableType == null)
            {
                return null;
            }

            var taskVariable = (TaskVariable<T>)Activator.CreateInstance(taskVariableType);
            taskVariable.data = internalData;
            return taskVariable;
        }
    }
}