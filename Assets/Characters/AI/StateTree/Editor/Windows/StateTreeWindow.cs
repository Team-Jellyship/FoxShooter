using System;
using FoxShooter.Characters.AI.StateTree.Graph;
using FoxShooter.Characters.AI.StateTree.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    internal class StateTreeWindow : EditorWindow
    {
        [SerializeField] public StateTreeGraph asset;

        private const string TreeViewFilename = "state-tree-view";
        private const string WindowFilename = "state-tree-window";
        
        private VisualTreeAsset _stateViewAsset;
        private VisualTreeAsset _windowAsset;

        private VisualElement _windowRoot;
        
        private StateTreeView _treeView;

        private static void Open(StateTreeGraph asset)
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
            // newWindow.asset = asset;
            newWindow.SetGraph(asset);
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
            _treeView = _windowRoot.Q<StateTreeView>("tree-view");
            rootVisualElement.Add(_windowRoot);
        }

        public void SetGraph(StateTreeGraph graph)
        {
            asset = graph;
            _treeView.Bind(graph);
        }

        private void LoadVisualAssets()
        {
            _windowAsset = Resources.Load<VisualTreeAsset>(WindowFilename);
        }

        [InitializeOnLoadMethod]
        private static void RegisterWindowDelegates()
        {
            StateTreeWindowDelegate.handler = Open;
        }
    }
}