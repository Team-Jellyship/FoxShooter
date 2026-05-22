using System;
using System.Collections.Generic;
using System.Reflection;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    [Serializable]
    public class TaskNode
    {
        [SerializeField] public string taskClassType;
        [SerializeReference] public List<TaskVariableNode> variables = new();

        public Task GenerateTask()
        {
            if (taskClassType == null)
            {
                Debug.LogError("[TaskNode] Failed to generate task. The Task type was null.");
                return null;
            }

            var taskType = Type.GetType(taskClassType);
            if (taskType == null)
            {
                Debug.LogError($"[TaskNode] Failed to generate task. Could not find type '{taskClassType}'");
                return null;
            }
            
            Debug.Log($"[TaskNode] Generating new '{taskType.Name} 'task from serialized data.");
            if (Activator.CreateInstance(taskType) is not Task newTask)
            {
                return null;
            }

            foreach (var taskVariable in taskType.GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance))
            {
                var node = variables.Find((variable) => variable.name == taskVariable.Name);
                if (node == null)
                {
                    continue;
                }
                taskVariable.SetValue(newTask, node.GenerateVariable());
            }

            return newTask;
        }

        public static TaskNode Serialize(Task task)
        {
            var taskNode = new TaskNode
            {
                taskClassType = task.GetType().FullName
            };
            
            foreach (var taskVariable in task.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance))
            {
                if (!taskVariable.FieldType.IsSubclassOf(typeof(TaskVariable)))
                {
                    continue;
                }

                var newVariable = MakeVariable(task, taskVariable);
                if (newVariable != null)
                {
                    taskNode.variables.Add(newVariable);
                }
            }
            return taskNode;
        }

        private static TaskVariableNode MakeVariable(Task task, FieldInfo taskVariable)
        {
            var taskVariableData = (TaskVariable) taskVariable.GetValue(task);

            var nodeType = GetSerializedType(taskVariableData);
            var serializeMethod = nodeType.GetMethod("Serialize", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);
            if (serializeMethod == null)
            {
                return null;
            }

            var args = new[] { taskVariable.Name, taskVariable.GetValue(task) };
            return (TaskVariableNode)serializeMethod.Invoke(task, args);
        }
        
        private static Type GetSerializedType(TaskVariable variable)
        {
            return variable.tag switch
            {
                TaskVariable.ContextTag.Blackboard => typeof(TaskVariableBlackboardReference<>).MakeGenericType(variable.type),
                TaskVariable.ContextTag.Variable => typeof(TaskVariableNode<>).MakeGenericType(variable.type),
                _ => null
            };
        }
    }
}