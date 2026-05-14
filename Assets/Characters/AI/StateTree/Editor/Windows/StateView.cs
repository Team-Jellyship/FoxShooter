using System.Linq;
using FoxShooter.Characters.AI.StateTree.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class StateView : VisualElement
    {
        private readonly TextField _title;
        private readonly Label _tasksLabel;
        private readonly Label _resultLabel;
        public readonly VisualElement stateContainer;
        public readonly VisualElement childContainer;
        private StateTreeView _treeView;
        
        public State state { get; private set; }

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

            stateContainer = new VisualElement
            {
                name = "state-container"
            };

            childContainer = new VisualElement()
            {
                name = "child-state-container"
            };
            
            stateContainer.Add(_title);
            stateContainer.Add(_tasksLabel);
            stateContainer.Add(_resultLabel);
            stateContainer.RegisterCallback<ClickEvent>(Clicked);
            
            Add(stateContainer);
            Add(childContainer);
        }

        public void Bind(State bindState, StateTreeView rootView)
        {
            if (bindState == null)
            {
                return;
            }
            
            for (var i = childContainer.childCount - 1; i >= 0; --i) 
            {
                childContainer.RemoveAt(i);
            }
            state = bindState;
            _treeView = rootView;
            _title.RegisterCallback<ChangeEvent<string>>((evt) =>
            { 
                state.name = evt.newValue;
            });
            
            SetNextStatesText();
            SetTasksText();

            foreach (var childState in state.childStates)
            {
                var childStateView = new StateView();
                new StateDragManipulator(childStateView, rootView);
                childContainer.Add(childStateView);
                childStateView.Bind(childState, rootView);
            }

            state.tasksChanged += SetTasksText;
            state.nextStatesChanged += SetNextStatesText;
        }

        public void SetTasksText()
        {
            if (state == null)
            {
                return;
            }
            
            _title.value = state.name;
            _tasksLabel.text = string.Join(", ", state.childTasks.Select(task => task.GetType().Name));
        }

        public void Select()
        {
            stateContainer.AddToClassList("selected");
        }

        public void Deselect()
        {
            stateContainer.RemoveFromClassList("selected");
        }

        private void Clicked(ClickEvent clickEvent)
        {
            Debug.Log($"Selected {state.name}");
            _treeView?.Select(this);
        }

        private void SetNextStatesText()
        {
            _resultLabel.text = "";
            if (state.successState != null)
            {
                _resultLabel.text += $"Success -> {state.successState.name}";

                if (state.cancelState != null)
                {
                    _resultLabel.text += ", ";
                }
            }
            if (state.cancelState != null)
            {
                _resultLabel.text += $"Cancel -> {state.cancelState.name}";
            }
        }
    }
}