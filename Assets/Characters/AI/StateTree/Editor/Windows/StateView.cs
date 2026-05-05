using FoxShooter.Characters.AI.StateTree;
using FoxShooter.Characters.AI.StateTree.Graph;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class StateView : VisualElement
    {
        private TextField _title;
        private Label _resultLabel;
        private Label _tasksLabel;

        private StateNode _state;

        public void Bind(StateNode state)
        {
            _state = state;
            _title = this.Q<TextField>("Title");
            _tasksLabel = this.Q<Label>("Tasks");
            _resultLabel = this.Q<Label>("Result");
            
            if (_title == null)
            {
                return;
            }

            _tasksLabel.text = string.Join(", ", state.tasks);
            
            _title.value = state.name;
            _title.RegisterCallback<ChangeEvent<string>>((evt) =>
            { 
                _state.name = evt.newValue;
            });
        }
    }
}