using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    public class BlackboardVariableSearch : PopupWindowContent
    {
        private const string BlackboardVariableSearchFilename = "blackboard-variable-search";

        public Action<BlackboardVariable> selected;

        private string _selectedVariableName;
        private Blackboard _blackboard;
        private VisualElement _tree;

        public override VisualElement CreateGUI()
        {
            var visualAsset = Resources.Load<VisualTreeAsset>(BlackboardVariableSearchFilename);
            _tree = visualAsset.CloneTree();
            _tree.AddToClassList("popup");
            return _tree;
        }

        public void SetBlackboardSearch(Blackboard blackboard, Type searchType)
        {
            _blackboard = blackboard;

            var stringConverter = new SelectionEntry<BlackboardVariable>.StringConverter(variable => variable.name);

            foreach (var blackboardVariable in blackboard.GetVariablesOfType(searchType))
            {
                var variableSelector = new SelectionEntry<BlackboardVariable>(blackboardVariable, stringConverter);
                variableSelector.RegisterCallback<MouseDownEvent>(_ => Selected(blackboardVariable));
                
                _tree.Add(variableSelector);
            }
        }

        private void Selected(BlackboardVariable blackboardVariable)
        {
            selected?.Invoke(blackboardVariable);
            editorWindow.Close();
        }
    }
}