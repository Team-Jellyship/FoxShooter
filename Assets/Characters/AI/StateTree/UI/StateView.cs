using FoxShooter.Characters.AI.StateTree;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    [UxmlElement]
    public partial class StateView : StateElement
    {
        private TextField _title;

        public override void Bind(State state)
        {
            base.Bind(state);

            _title = this.Q<TextField>("Title");

            if (_title == null)
            {
                return;
            }
            
            _title.value = state.name;
            _title.RegisterCallback<ChangeEvent<string>>((evt) =>
            { 
                data.name = evt.newValue;
            });
        }
    }
}