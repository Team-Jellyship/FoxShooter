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
        [SerializeReference] public Type taskClassType;
        [SerializeReference] public List<TaskVariableNode> variables;

        public Task GenerateTask()
        {
            if (Activator.CreateInstance(taskClassType) is not Task newTask)
            {
                return null;
            }

            foreach (var taskVariable in taskClassType.GetFields(
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
                taskClassType = task.GetType()
            };
            
            foreach (var taskVariable in task.GetType().GetFields(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance))
            {
                if (taskVariable.FieldType != typeof(TaskVariable))
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