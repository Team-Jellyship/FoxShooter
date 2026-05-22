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
        private Blackboard _blackboard;
        private TaskVariable _taskVariable;
        
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
            _blackboard = blackboard;
            _taskVariable = taskVariable;
            
            style.flexDirection = FlexDirection.Row;
            style.unityTextAlign = TextAnchor.MiddleCenter;
            _variableField = new GenericField(taskVariable.type, taskVariable.data, name)
            {
                dataChanged = data => { taskVariable.data = data; }
            };
            _selectorButton = new Button
            {
                name = "task-variable-selector-button",
                text = "▾"
            };
            _selectorButton.clicked += Clicked;
            
            Add(_variableField);
            Add(_selectorButton);
        }

        private void Clicked()
        {
            var currentSearch = new BlackboardVariableSearch();
            // currentSearch.selected += AddNewBlackboardVariable;
            UnityEditor.PopupWindow.Show(_selectorButton.worldBound, currentSearch);
            currentSearch.SetBlackboardSearch(_blackboard, _taskVariable.type);
            currentSearch.selected = VariableSelected;
        }

        private void VariableSelected(BlackboardVariable variable)
        {
            var blackboardReference = new BlackboardReference<float>(_blackboard, variable.name);
            _taskVariable.Set(blackboardReference);
            Debug.Log($"Selected variable {variable.name}");
        }
    }
}