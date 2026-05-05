using System;
using System.Collections.Generic;
using System.Linq;
using FoxShooter.Characters.AI.StateTree.Graph;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    public class StateTreeView : VisualElement
    {
        private const string DraggedItemsKey = "DraggedIndices";
        private const string SourceKey = "SourceCollection";
        private const int MaxDepth = 10;

        private readonly TreeView _treeView;

        private readonly VisualTreeAsset _stateEntryAsset;
        private readonly List<int> _usedIndices = new();

        private StateTreeGraph _tree;

        public StateTreeView(VisualElement rootVisualElement, VisualTreeAsset stateEntryAsset, StateTreeGraph tree)
        {

        }
    }
}