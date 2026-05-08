using System.Collections.Generic;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public sealed partial class StateSelector : PopupField<State>
    {
        public StateSelector()
        {
            formatListItemCallback = FormatListItem;
            formatSelectedValueCallback = FormatListItem;
        }
        
        public void Bind(Tree tree, State owningState, State selectedState)
        {
            choices = tree.GetAllStates();
            choices.Remove(owningState);
            choices.Add(null);
            
            index = choices.FindIndex(state => state == selectedState);
        }

        private static string FormatListItem(State state)
        {
            return state == null ? "None" : state.GetFullName();
        }
    }
}