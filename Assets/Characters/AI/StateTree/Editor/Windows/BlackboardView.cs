using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class BlackboardView : VisualElement
    {
        private Blackboard _blackboard;

        private VisualElement _blackboardVariableContainer;

        public BlackboardView()
        {
            _blackboardVariableContainer = new VisualElement
            {
                name = "blackboard-variable-container"
            };
            Add(_blackboardVariableContainer);
        }
        
        public void Bind(Blackboard blackboard)
        {
            for (var i = _blackboardVariableContainer.childCount - 1; i >= 0; --i)
            {
                _blackboardVariableContainer.RemoveAt(i);
            }

            _blackboard = blackboard;
            
            Debug.Log($"[BlackboardView] Binding blackboard to view with {blackboard.variables.Count} variables.");
            foreach (var blackboardVariable in blackboard.variables.Values)
            {
                var blackboardVariableView = new BlackboardVariableView();
                blackboardVariableView.Bind(blackboard, blackboardVariable);
                blackboardVariableView.name = "blackboard-variable-view";
                _blackboardVariableContainer.Add(blackboardVariableView);
            }
        }
    }
}