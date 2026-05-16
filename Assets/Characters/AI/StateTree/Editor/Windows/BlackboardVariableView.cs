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

        private readonly ToggleRenameableField _name;
        private readonly GenericField _field;
        
        public BlackboardVariableView()
        {
            _name = new ToggleRenameableField("BlackboardVariable");
            _field = new GenericField();
            Add(_name);
            Add(_field);
        }

        public BlackboardVariableView(Blackboard board, BlackboardVariable blackboardVariable)
        {
            _parentBlackboard = board;
            variable = blackboardVariable;
            _name = new ToggleRenameableField(blackboardVariable.name);
            _field = new GenericField(blackboardVariable.type, blackboardVariable.objectData);
            _name.renamed = newName =>
            {
                if (board.RenameVariable(blackboardVariable.name, newName))
                {
                    _name.text = newName;
                }
            };
            _field.dataChanged += data =>
            { variable.objectData = data; }; 
            Add(_name);
            Add(_field);
        }

        public void StartEditing()
        {
            _name.StartEditing();
        }
    }
}