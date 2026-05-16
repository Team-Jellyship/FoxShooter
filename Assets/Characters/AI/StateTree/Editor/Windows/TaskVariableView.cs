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

        public TaskVariableView(string name, TaskVariable taskVariable)
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
            
            Add(_variableField);
            Add(_selectorButton);
        }
    }
}