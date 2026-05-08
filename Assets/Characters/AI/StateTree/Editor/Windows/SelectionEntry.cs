using System;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    public class SelectionEntry<T> : VisualElement
    {
        public delegate string StringConverter(T objectToConvert);

        private T _selection;

        private Label _label;

        public SelectionEntry(T displayedSelection, StringConverter converter)
        {
            _selection = displayedSelection;

            _label = new Label
            {
                text = converter(displayedSelection)
            };
            _label.AddToClassList("selection-entry");
            Add(_label);
        }
    }
}