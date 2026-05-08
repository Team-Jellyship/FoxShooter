using Characters.AI.StateTree.Editor.Windows;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class StateInspector : VisualElement
    {
        private State _activeState;

        private Label _stateNameLabel;
        private StateSelector _successSelector;
        private StateSelector _cancelSelector;
        private VisualElement _taskContainer;
        
        public StateInspector()
        {
            _stateNameLabel = new Label
            {
                name = "state-name-label",
                text = "State"
            };

            _successSelector = new StateSelector
            {
                name = "success-state-selector"
            };

            _cancelSelector = new StateSelector
            {
                name = "cancel-state-selector"
            };

            _taskContainer = new VisualElement
            {
                name = "task-container"
            };
            
            
            _successSelector.RegisterCallback<ChangeEvent<State>>(changeEvent => SetSuccessState(changeEvent.newValue));
            _cancelSelector.RegisterCallback<ChangeEvent<State>>(changeEvent => SetCancelState(changeEvent.newValue));
            
            Add(_stateNameLabel);
            Add(_successSelector);
            Add(_cancelSelector);
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
            _successSelector.Bind(tree, state, state.successState);
            _cancelSelector.Bind(tree, state, state.cancelState);
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