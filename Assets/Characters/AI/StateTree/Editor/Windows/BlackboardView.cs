using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    public class BlackboardView
    {
        private Blackboard _blackboard;

        private VisualElement _blackboardVariableContainer;
        private Button _blackboardVariableAdder;
        private TypeSearch _currentSearch;

        public BlackboardView(Blackboard blackboard, VisualElement rootElement)
        {
            _blackboardVariableContainer = rootElement.Q<VisualElement>("blackboard-variable-container");
            _blackboardVariableAdder = rootElement.Q<Button>("blackboard-variable-button");
            
            _blackboard = blackboard;
            _blackboard.onVariablesChanged = UpdateVariables;

            _blackboardVariableAdder.clicked += DisplayBlackboardTypeSelector;
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
                var blackboardVariableView = new BlackboardVariableView(_blackboard, blackboardVariable)
                {
                    name = "blackboard-variable-view"
                };
                _blackboardVariableContainer.Add(blackboardVariableView);
            }
        }

        private void DisplayBlackboardTypeSelector()
        {
            _currentSearch = new TypeSearch();
            _currentSearch.selected += AddNewBlackboardVariable;
            UnityEditor.PopupWindow.Show(_blackboardVariableAdder.worldBound, _currentSearch);
        }

        private void AddNewBlackboardVariable(Type type)
        {
            var blackboardVariableType = typeof(BlackboardVariable<>).MakeGenericType(type);
            var newVariable = (BlackboardVariable)Activator.CreateInstance(blackboardVariableType, "test");
            if (newVariable == null)
            {
                return;
            }
            _blackboard.AddVariable(newVariable);
        }
    }
}