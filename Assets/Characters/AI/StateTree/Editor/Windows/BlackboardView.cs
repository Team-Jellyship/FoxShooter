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
            
            _blackboard = blackboard;
            _blackboard.onVariablesChanged = UpdateVariables;

            _blackboardVariableAdder.clicked += () =>
            {
                _blackboard.AddVariable<int>("test");
            };
            UpdateVariables();
        }

        private void UpdateVariables()
        {
            for (var i = _blackboardVariableContainer.childCount - 1; i >= 0; --i)
            {
                _blackboardVariableContainer.RemoveAt(i);
            }
            foreach (var blackboardVariable in _blackboard.variables.Values)
            {
                var blackboardVariableView = new BlackboardVariableView();
                blackboardVariableView.Bind(_blackboard, blackboardVariable);
                blackboardVariableView.name = "blackboard-variable-view";
                _blackboardVariableContainer.Add(blackboardVariableView);
            }
        }
    }
}