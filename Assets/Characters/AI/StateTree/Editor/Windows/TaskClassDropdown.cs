using System;
using System.Linq;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    [UxmlElement]
    public sealed partial class TaskClassDropdown : PopupField<Type>
    {
        public TaskClassDropdown()
        {
            var type = typeof(Tasks.Task);
            choices = type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type) && t != type).ToList();
        }
    }
}