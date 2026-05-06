using System.Linq;
using FoxShooter.Characters.AI.StateTree;
using FoxShooter.Characters.AI.StateTree.Graph;
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
                name = "title"
            };
            
            _tasksLabel= new Label
            {
                name = "tasks-label"
            };
            
            _resultLabel = new Label
            {
                name = "result-label"
            };

            _stateContainer = new VisualElement
            {
                name = "state-container"
            };

            _childContainer = new VisualElement();
            _childContainer.AddToClassList("child-state-container");
            
            _stateContainer.Add(_title);
            _stateContainer.Add(_tasksLabel);
            _stateContainer.Add(_resultLabel);
            
            Add(_stateContainer);
            Add(_childContainer);
        }

        public void Bind(State state)
        {
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