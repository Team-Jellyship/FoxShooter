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
        private readonly GenericField _field;
        
        public BlackboardVariableView()
        {
            _field = new GenericField();
            Add(_field);
        }

        public BlackboardVariableView(Blackboard board, BlackboardVariable blackboardVariable)
        {
            _parentBlackboard = board;
            variable = blackboardVariable;
            _field = new GenericField(blackboardVariable.name, blackboardVariable.type, blackboardVariable.objectData);
            _field.dataChanged += data =>
            { variable.objectData = data; }; 
            Add(_field);
        }
    }
}