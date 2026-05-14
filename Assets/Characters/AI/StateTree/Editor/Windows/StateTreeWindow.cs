using System;
using FoxShooter.Characters.AI.StateTree.Graph;
using FoxShooter.Characters.AI.StateTree.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.Editor.Windows
{
    internal class StateTreeWindow : EditorWindow
    {
        [SerializeField] public StateTreeGraph asset;

        private Tree _tree;

        private const string WindowFilename = "state-tree-window";
        private VisualTreeAsset _windowAsset;

        private VisualElement _windowRoot;
        
        private StateTreeView _treeView;
        private BlackboardView _blackboardView;
        private StateInspector _stateInspector;
        
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
            _windowRoot.style.height = Length.Percent(100);

            foreach (var placeholder in _windowRoot.Query(classes: "placeholder").ToList())
            {
                placeholder.RemoveFromHierarchy();
            }
            
            _treeView = _windowRoot.Q<StateTreeView>("tree-view");
            _stateInspector = _windowRoot.Q<StateInspector>("state-inspector");
            
            _button = _windowRoot.Q<Button>("save-button");
            _button.clicked += Save;

            _addStateButton = _windowRoot.Q<Button>("new-state-button");
            _addStateButton.clicked += () =>
            {
                _treeView?.AddState();
            };
            
            rootVisualElement.Add(_windowRoot);

            if (!asset)
            {
                return;
            }
            
            SetGraph(asset);
        }

        public void SetGraph(StateTreeGraph graph)
        {
            asset = graph;
            _tree = asset.GenerateTree();
            _treeView?.Bind(_tree);
            _blackboardView = new BlackboardView(graph.blackboard, _windowRoot.Q<VisualElement>("blackboard-view"));
            _treeView?.selectedStateChanged.AddListener(SelectedStateChanged);
            _stateInspector.visible = false;
        }

        private void LoadVisualAssets()
        {
            _windowAsset = Resources.Load<VisualTreeAsset>(WindowFilename);
        }

        private void SelectedStateChanged(State selectedState)
        {
            _stateInspector.visible = selectedState != null;
            _stateInspector.Bind(_tree, selectedState);
        }
        
        private void Save()
        {
            if (!asset)
            {
                return;
            }

            var serializedVersion = StateTreeGraph.SerializeTree(_tree);
            asset.nodes = serializedVersion.nodes;
            asset.rootNode = serializedVersion.rootNode;
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssetIfDirty(asset);
        }

        [InitializeOnLoadMethod]
        private static void RegisterWindowDelegates()
        {
            StateTreeWindowDelegate.handler = Open;
        }
    }
}