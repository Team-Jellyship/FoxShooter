using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class TaskVariableView : VisualElement
    {
        private GenericField _variableField;
        private Button _selectorButton;
        
        public TaskVariableView()
        {
            style.flexDirection = FlexDirection.Row;
            style.unityTextAlign = TextAnchor.MiddleCenter;

            _variableField = new GenericField(typeof(string), "default", "Task Variable");
            _selectorButton = new Button
            {
                name = "task-variable-selector-button",
                text = "▾"
            };
            
            Add(_variableField);
            Add(_selectorButton);
        }

        public TaskVariableView(string name, TaskVariable taskVariable, Blackboard blackboard)
        {
            style.flexDirection = FlexDirection.Row;
            style.unityTextAlign = TextAnchor.MiddleCenter;
            _variableField = new GenericField(taskVariable.type, taskVariable.data, name);
            _variableField.dataChanged = data => { taskVariable.data = data; };
            _selectorButton = new Button
            {
                name = "task-variable-selector-button",
                text = "▾"
            };
            _selectorButton.clicked += () =>
            {
                var currentSearch = new BlackboardVariableSearch();
                // currentSearch.selected += AddNewBlackboardVariable;
                UnityEditor.PopupWindow.Show(_selectorButton.worldBound, currentSearch);
                currentSearch.SetBlackboardSearch(blackboard, taskVariable.type);
                currentSearch.selected = variable =>
                {
                    Debug.Log($"Selected variable {variable.name}");
                };
            };
            
            Add(_variableField);
            Add(_selectorButton);
        }
    }
}