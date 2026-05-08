using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.TextureAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using PopupWindow = UnityEditor.PopupWindow;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    
    public class TaskSearch : PopupWindowContent
    {
        private const string TaskSearchFilename = "task-search";

        public Action<Type> selected;
            
        private SelectionEntry<Type> _currentlySelectedElement;

        public override VisualElement CreateGUI()
        {
            var visualAsset = Resources.Load<VisualTreeAsset>(TaskSearchFilename);
            var tree = visualAsset.CloneTree();
            
            var stringConverter = new SelectionEntry<Type>.StringConverter(type => type.Name);
            
            foreach (var task in GetTaskTypes())
            {
                var taskSelector = new SelectionEntry<Type>(task, stringConverter);
                taskSelector.RegisterCallback<MouseDownEvent>(_ => Selected(task));
                tree.Add(taskSelector);
            }
            
            return tree;
        }

        public static List<Type> GetTaskTypes()
        {
            var type = typeof(Tasks.Task);
            return type.Assembly.GetTypes().Where(t => t.IsSubclassOf(type) && t != type).ToList();
        }

        private void Selected(Type taskType)
        {
            selected(taskType);
            editorWindow.Close();
        }
    }
}