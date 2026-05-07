using System.Linq;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class StateView : VisualElement
    {
        private readonly TextField _title;
        private readonly Label _tasksLabel;
        private readonly Label _resultLabel;
        private readonly VisualElement _stateContainer;
        private readonly VisualElement _childContainer;
        private State _state;

        public StateView()
        {
            _title = new TextField
            {
                value = "Root",
                name = "title"
            };
            
            _tasksLabel= new Label
            {
                text = "task1, task2",
                name = "tasks-label"
            };
            
            _resultLabel = new Label
            {
                text = "Success -> ",
                name = "result-label"
            };

            _stateContainer = new VisualElement
            {
                name = "state-container"
            };

            _childContainer = new VisualElement()
            {
                name = "child-state-container"
            };
            
            _stateContainer.Add(_title);
            _stateContainer.Add(_tasksLabel);
            _stateContainer.Add(_resultLabel);
            
            Add(_stateContainer);
            Add(_childContainer);
        }

        public void Bind(State state)
        {
            for (var i = _childContainer.childCount - 1; i >= 0; --i) 
            {
                _childContainer.RemoveAt(i);
            }
            _state = state;
            
            _title.RegisterCallback<ChangeEvent<string>>((evt) =>
            { 
                _state.name = evt.newValue;
            });
            
            Update();

            foreach (var childState in state.childStates)
            {
                var childStateView = new StateView();
                _childContainer.Add(childStateView);
                childStateView.Bind(childState);
            }
        }

        public void Update()
        {
            if (_state == null)
            {
                return;
            }
            
            _title.value = _state.name;
            _tasksLabel.text = string.Join(", ", _state.childTasks.Select(task => task.GetType().Name));
        }
    }
}