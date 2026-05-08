using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public partial class TaskAdder : VisualElement
    {
        public Action<Type> typeAddRequested; 
        private Button _addButton;
        private TaskSearch _currentSearch;

        public TaskAdder()
        {
            _addButton = new Button
            {
                name = "task-adder-button",
                text = "+"
            };
            
            _addButton.clicked += Clicked;
            
            Add(_addButton);
        }

        private void Clicked()
        {
            _currentSearch = new TaskSearch();
            _currentSearch.selected += taskType => typeAddRequested(taskType);
            UnityEditor.PopupWindow.Show(_addButton.worldBound, _currentSearch);
        }
    }
}