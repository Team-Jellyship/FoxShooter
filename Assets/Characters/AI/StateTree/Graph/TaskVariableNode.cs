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
            var result = new TaskVariableNode<T>
            {
                name = name,
                type = taskVariable.GetType().FullName,
                internalData = (T)taskVariable.data,
            };
            // result.tag = taskVariable.tag;
            return result;
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