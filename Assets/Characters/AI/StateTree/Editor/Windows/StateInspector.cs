using System;
using Characters.AI.StateTree.Editor.Windows;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class StateInspector : VisualElement
    {
        private State _activeState;

        private Label _stateNameLabel;
        private StateDropdown _successDropdown;
        private StateDropdown _cancelDropdown;
        private Label _taskTitle;
        private TaskAdder _taskAdder;
        private VisualElement _taskContainer;
        
        public StateInspector()
        {
            _stateNameLabel = new Label
            {
                name = "state-name-label",
                text = "State"
            };

            _successDropdown = new StateDropdown
            {
                name = "success-state-selector",
                label = "Success"
            };

            _cancelDropdown = new StateDropdown
            {
                name = "cancel-state-selector",
                label = "Cancel"
            };

            _taskTitle = new Label
            {
                name = "task-title",
                text = "Tasks"
            };

            _taskAdder = new TaskAdder
            {
                name = "task-adder"
            };

            _taskContainer = new VisualElement
            {
                name = "task-container"
            };
            
            
            _successDropdown.RegisterCallback<ChangeEvent<State>>(changeEvent => SetSuccessState(changeEvent.newValue));
            _cancelDropdown.RegisterCallback<ChangeEvent<State>>(changeEvent => SetCancelState(changeEvent.newValue));
            _taskAdder.typeAddRequested += AddTask;
            
            Add(_stateNameLabel);
            Add(_successDropdown);
            Add(_cancelDropdown);
            Add(_taskTitle);
            Add(_taskAdder);
            Add(_taskContainer);
        }

        public void Bind(Tree tree, State state)
        {
            if (state == _activeState)
            {
                return;
            }
            
            if (_activeState != null)
            {
                _activeState.tasksChanged -= UpdateTasks;
            }
            
            _activeState = state;

            if (state == null)
            {
                _stateNameLabel.text = "";
                return;
            }
            
            _stateNameLabel.text = state.name;
            _successDropdown.Bind(tree, state, state.successState);
            _cancelDropdown.Bind(tree, state, state.cancelState);
            state.tasksChanged += UpdateTasks;
            
            UpdateTasks();
        }

        public void UpdateTasks()
        {
            for (var i = _taskContainer.childCount - 1; i >= 0; --i)
            {
                _taskContainer.RemoveAt(i);
            }

            if (_activeState == null)
            {
                return;
            }
            
            foreach (var task in _activeState.childTasks)
            {
                var taskView = new TaskView(task);
                _taskContainer.Add(taskView);
            }
        }

        private void SetSuccessState(State state)
        {
            _activeState?.SetSuccessState(state);
        }

        private void SetCancelState(State state)
        {
            _activeState?.SetCancelState(state);
        }

        private void AddTask(Type taskType)
        {
            var newTask = Activator.CreateInstance(taskType);
            if (newTask is Task task)
            {
                _activeState.AddTask(task);
            }
        }
    }
}