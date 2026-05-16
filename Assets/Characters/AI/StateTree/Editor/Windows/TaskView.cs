using System;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class TaskView : VisualElement
    {
        private Label _taskNameLabel;
        private VisualElement _taskVariableContainer;

        private Task _task;

        public TaskView()
        {
            _taskNameLabel = new Label
            {
                name = "task-name",
                text = "Task"
            };

            _taskVariableContainer = new VisualElement
            {
                name = "task-variable-container"
            };
            
            Add(_taskNameLabel);
            Add(_taskVariableContainer);
        }

        public TaskView(Task task)
        {
            _taskNameLabel = new Label
            {
                name = "task-name",
            };

            _taskVariableContainer = new VisualElement
            {
                name = "task-variable-container"
            };
            
            Add(_taskNameLabel);
            Add(_taskVariableContainer);
            Bind(task);
        }

        public void Bind(Task task)
        {
            _task = task;
            _taskNameLabel.text = task.ToString();

            for (var i = _taskVariableContainer.childCount - 1; i >= 0; --i)
            {
                _taskVariableContainer.RemoveAt(i);
            }

            foreach (var taskVariable in task.GetClassVariables())
            {
                /*var newField = new GenericField(taskVariable.Item2.type, taskVariable.Item2.data);
                newField.dataChanged = data =>
                {
                    Debug.Log("Variable changed");
                    taskVariable.Item2.data = data;
                };*/
                var newField = new TaskVariableView(taskVariable.Item1, taskVariable.Item2);
                _taskVariableContainer.Add(newField);
            }
        }
    }
}