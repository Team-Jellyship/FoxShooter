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
                taskVariable.SetValue(newTask, node.variable);
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
                taskNode.variables.Add(new TaskVariableNode
                {
                    name = taskVariable.Name,
                    variable = (TaskVariable)taskVariable.GetValue(task)
                });
            }
            return taskNode;
        }
    }
}