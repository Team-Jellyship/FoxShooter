using FoxShooter.Characters.AI.StateTree;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    [UxmlElement]
    public partial class StateElement : VisualElement
    {
        public State data { get; private set; }
        public int id;

        public virtual void Bind(State state)
        {
            data = state;
        }

        public virtual void Reset()
        {
            data = null;
        }
    }
}