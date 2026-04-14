using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    public class StateTreeGraphEditor : EditorWindow
    {
        private const string ResourceFilename = "StateTreeGraphEditor";
        private const string StateTreeViewFilename = "StateTreeView";
        private const string StateTreeEntryFilename = "StateView";

        private StateTreeGraph _stateTreeGraph;

        private VisualElement _stateTreeView;
        
        private VisualTreeAsset _stateView;
        private VisualTreeAsset _stateTreeViewAsset;
        private StateTreeView _treeView;

        public void CreateGUI()
        {
            var visualTree = Resources.Load<VisualTreeAsset>(ResourceFilename);
            var styleSheet = Resources.Load<StyleSheet>(ResourceFilename);
            _stateTreeViewAsset = Resources.Load<VisualTreeAsset>(StateTreeViewFilename);
            _stateView = Resources.Load<VisualTreeAsset>(StateTreeEntryFilename);

            if (!visualTree || !styleSheet)
            {
                return;
            }
            rootVisualElement.styleSheets.Add(styleSheet);

            visualTree.CloneTree(rootVisualElement);
            var indented = rootVisualElement.Query<VisualElement>("Indentation");
            var newStateButton = rootVisualElement.Query<Button>("NewStateButton");
            newStateButton.First().clicked += AddNewState;
            rootVisualElement.Query<Button>("SaveButton").First().clicked += Save;
        }

        public void LoadState(StateTreeGraph graph)
        {
            _stateTreeGraph = graph;
            Rebuild();
        }

        private void AddNewState()
        {
            _treeView.AddState();
            
            Rebuild();
        }

        private void Rebuild()
        {
            _stateTreeView?.RemoveFromHierarchy();
            _stateTreeView = new VisualElement();
            _stateTreeViewAsset.CloneTree(_stateTreeView);
            rootVisualElement.Q("TreeContainer").Add(_stateTreeView);
            _treeView = new StateTreeView(_stateTreeView, _stateView, _stateTreeGraph);
            SaveChanges();
        }

        private void Save()
        {
            _treeView.Save();
        }
    }
}