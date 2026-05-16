using System;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class ToggleRenameableField : VisualElement
    {
        public Action<string> renamed;

        public string text
        {
            set => _label.text = value;
            get => _label.text;
        }
        
        private TextField _field;
        private readonly Label _label;
        private bool _editing = false;
        
        public ToggleRenameableField()
        {
            _label = new Label();
            
            Add(_label);
        }

        public ToggleRenameableField(string text)
        {
            _label = new Label { text = text };
            _label.RegisterCallback<ClickEvent>(clickEvent =>
            { 
                if (clickEvent.clickCount == 2)
                {
                    StartEditing();
                }
            });
            Add(_label);
        }

        public void StartEditing()
        {
            if (_editing)
            {
                return;
            }

            _editing = true;
            _label.style.display = DisplayStyle.None;
            _field = new TextField
            {
                value = text
            };
            _field.RegisterCallback<FocusOutEvent>(FocusLost);
            Add(_field);
            _field.Focus();
        }

        private void FocusLost(FocusOutEvent focusOutEvent)
        {
            if (!_editing)
            {
                return;
            }
            _editing = false;
            _label.style.display = DisplayStyle.Flex;
            renamed?.Invoke(_field.value);
            Remove(_field);
        }
    }
}