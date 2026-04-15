using FoxShooter.Characters.AI.StateTree;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    [UxmlElement]
    public partial class StateView : StateElement
    {
        private TextField _title;
        private Label _resultLabel;
        private Label _tasksLabel;

        public override void Bind(State state, StateTreeGraph graph)
        {
            base.Bind(state, graph);

            _title = this.Q<TextField>("Title");
            _tasksLabel = this.Q<Label>("Tasks");
            _resultLabel = this.Q<Label>("Result");
            
            if (_title == null)
            {
                return;
            }

            _tasksLabel.text = string.Join(", ", state.childTasks);
            
            _title.value = state.name;
            _title.RegisterCallback<ChangeEvent<string>>((evt) =>
            { 
                data.name = evt.newValue;
            });

            _resultLabel.text = null;
            if (graph.TryGetState(state.successStateIndex, out var success))
            {
                _resultLabel.text += $"✓ {success.name} ";
            }
            if (graph.TryGetState(state.cancelStateIndex, out var cancel))
            {
                _resultLabel.text += $"x {cancel.name}";
            }
        }
    }
}