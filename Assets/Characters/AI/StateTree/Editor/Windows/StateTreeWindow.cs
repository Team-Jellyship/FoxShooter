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

        private const string WindowFilename = "state-tree-window";
        
        private VisualTreeAsset _windowAsset;

        private VisualElement _windowRoot;
        
        private StateTreeView _treeView;
        private BlackboardView _blackboardView;
        
        private Button _button;
        private Button _addStateButton;

        private static void Open(StateTreeGraph asset)
        {
            var windows = Resources.FindObjectsOfTypeAll<StateTreeWindow>();
            foreach (var window in windows)
            {
                if (window.asset != asset) { continue; }
                window.SetGraph(asset);
                window.Show();
                window.Focus();
                return;
            }
            
            var newWindow = CreateWindow<StateTreeWindow>(typeof(StateTreeWindow));
            newWindow.titleContent.text = asset.name;
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

            foreach (var placeholder in _windowRoot.Query(classes: "placeholder").ToList())
            {
                placeholder.RemoveFromHierarchy();
            }
            
            _treeView = _windowRoot.Q<StateTreeView>("tree-view");
            _blackboardView = _windowRoot.Q<BlackboardView>("blackboard-view");
            
            _button = _windowRoot.Q<Button>("save-button");
            _button.clicked += () =>
            {
                _treeView?.Save();
            };

            _addStateButton = _windowRoot.Q<Button>("new-state-button");
            _addStateButton.clicked += () =>
            {
                _treeView?.AddState();
            };
            
            rootVisualElement.Add(_windowRoot);

            if (asset)
            {
                _treeView.Bind(asset);
            }
        }

        public void SetGraph(StateTreeGraph graph)
        {
            asset = graph;
            _treeView?.Bind(graph);
            _blackboardView.Bind(graph.blackboard);
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