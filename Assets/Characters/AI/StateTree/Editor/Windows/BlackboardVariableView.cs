using System;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class BlackboardVariableView : VisualElement
    {
        public BlackboardVariable variable { get; private set; }
        public Delegate onDeleted;

        private Blackboard _parentBlackboard;
        private readonly TextField _name;
        private VisualElement _field;
        
        public BlackboardVariableView()
        {
            _name = new TextField
            {
                value = "Blackboard Variable",
                name = "blackboard-variable-name"
            };
            _name.RegisterCallback<FocusOutEvent>(_ => ChangeName());

            var placeholderField = new TextField
            {
                value = "default",
                name = "blackboard-variable-field"
            };
            _field = placeholderField;

            Add(_name);
            Add(_field);
        }

        public void Bind(Blackboard board, BlackboardVariable blackboardVariable)
        {
            _parentBlackboard = board;
            variable = blackboardVariable;
            _name.value = blackboardVariable.name;
        }

        private void ChangeName()
        {
            if (_parentBlackboard == null)
            {
                return;
            }

            if (_parentBlackboard.RenameVariable(variable.name, _name.value))
            {
                return;
            }
            
            // Failed to change name, undo the change in the field
            _name.value = variable.name;
        }
    }
}