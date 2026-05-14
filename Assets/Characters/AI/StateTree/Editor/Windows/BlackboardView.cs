using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    public class BlackboardView
    {
        private Blackboard _blackboard;

        private VisualElement _blackboardVariableContainer;
        private Button _blackboardVariableAdder;

        public BlackboardView(Blackboard blackboard, VisualElement rootElement)
        {
            _blackboardVariableContainer = rootElement.Q<VisualElement>("blackboard-variable-container");
            _blackboardVariableAdder = rootElement.Q<Button>("blackboard-variable-button");
        
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