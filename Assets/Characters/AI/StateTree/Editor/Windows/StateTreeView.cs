using System;
using System.Collections.Generic;
using System.Linq;
using FoxShooter.Characters.AI.StateTree.Editor.Windows;
using FoxShooter.Characters.AI.StateTree.Graph;
using Unity.AppUI.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    [UxmlElement]
    public partial class StateTreeView : VisualElement
    {
        private const string DraggedItemsKey = "DraggedIndices";
        private const string SourceKey = "SourceCollection";
        private const int MaxDepth = 10;

        private StateTreeGraph treeGraph;
        private Tree _tree;
        private VisualElement _container;
        private StateView _rootView;

        public void Bind(StateTreeGraph tree)
        {
            treeGraph = tree;
            _tree = tree.GenerateTree();

            if (_tree == null)
            {
                return;
            }
            
            _rootView = new StateView();
            _rootView.Bind(_tree.root);
            Add(_rootView);
        }

        public void Save()
        {
            if (!treeGraph)
            {
                return;
            }

            var serializedVersion = StateTreeGraph.SerializeTree(_tree);
            treeGraph.nodes = serializedVersion.nodes;
            treeGraph.rootNode = serializedVersion.rootNode;
            EditorUtility.SetDirty(treeGraph);
            AssetDatabase.SaveAssetIfDirty(treeGraph);
        }

        public void AddState()
        {
            if (_tree == null)
            {
                return;
            }
            
            _tree.AddState("New State");
            _rootView.Bind(_tree.root);
            EditorUtility.SetDirty(treeGraph);
        }
    }
}