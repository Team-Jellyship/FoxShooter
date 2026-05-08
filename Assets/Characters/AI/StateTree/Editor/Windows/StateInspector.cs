using Characters.AI.StateTree.Editor.Windows;
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

            _taskContainer = new VisualElement
            {
                name = "task-container"
            };
            
            
            _successDropdown.RegisterCallback<ChangeEvent<State>>(changeEvent => SetSuccessState(changeEvent.newValue));
            _cancelDropdown.RegisterCallback<ChangeEvent<State>>(changeEvent => SetCancelState(changeEvent.newValue));
            
            Add(_stateNameLabel);
            Add(_successDropdown);
            Add(_cancelDropdown);
            Add(_taskTitle);
            Add(_taskContainer);
        }

        public void Bind(Tree tree, State state)
        {
            _activeState = state;

            if (state == null)
            {
                _stateNameLabel.text = "";
                return;
            }
            
            _stateNameLabel.text = state.name;
            _successDropdown.Bind(tree, state, state.successState);
            _cancelDropdown.Bind(tree, state, state.cancelState);
        }

        private void SetSuccessState(State state)
        {
            _activeState.successState = state;
        }

        private void SetCancelState(State state)
        {
            _activeState.cancelState = state;
        }
    }
}