using System;
using FoxShooter.Characters.AI.StateTree.Graph;
using FoxShooter.Characters.AI.StateTree.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    internal class StateTreeWindow : EditorWindow
    {
        [SerializeField] public StateTreeGraph asset;

        private const string TreeViewFilename = "state-tree-view";
        private const string StateViewFilename = "state-view";
        private const string WindowFilename = "state-tree-window";
        
        private VisualTreeAsset _treeViewAsset;
        private VisualTreeAsset _stateViewAsset;
        private VisualTreeAsset _windowAsset;

        private VisualElement _windowRoot;
        
        private StateTreeView _treeView;

        static void Open(StateTreeGraph asset)
        {
            var windows = Resources.FindObjectsOfTypeAll<StateTreeWindow>();
            foreach (var window in windows)
            {
                if (window.asset != asset) { continue; }
                window.Show();
                window.Focus();
                return;
            }

            var newWindow = CreateWindow<StateTreeWindow>(typeof(StateTreeWindow));
            newWindow.titleContent.text = asset.name;
            newWindow.asset = asset;
            newWindow.Show();
            newWindow.Focus();
        }

        private static bool IsAssetValid(Tree asset)
        {
            return asset != null;
        }
        
        public void CreateGUI()
        {
            LoadVisualAssets();
            
            _windowRoot = _windowAsset.CloneTree();
            rootVisualElement.Add(_windowRoot);
        }

        private void LoadVisualAssets()
        {
            _treeViewAsset = Resources.Load<VisualTreeAsset>(TreeViewFilename);
            _stateViewAsset = Resources.Load<VisualTreeAsset>(StateViewFilename);
            _windowAsset = Resources.Load<VisualTreeAsset>(WindowFilename);
        }

        [InitializeOnLoadMethod]
        private static void RegisterWindowDelegates()
        {
            StateTreeWindowDelegate.handler = Open;
        }
    }
}