using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.Characters.AI.StateTree.UI
{
    public class StateTreeView
    {
        private const string DraggedItemsKey = "DraggedIndices";
        private const string SourceKey = "SourceCollection";
        private const int MaxDepth = 10;

        private readonly TreeView _treeView;

        private readonly List<TreeViewItemData<State>> _stateTreeItems = new();
        private readonly VisualTreeAsset _stateEntryAsset;
        private readonly List<int> _usedIndices = new();

        private StateTreeGraph _graph;

        public StateTreeView(VisualElement rootVisualElement, VisualTreeAsset stateEntryAsset, StateTreeGraph graph)
        {
            _graph = graph;
            _stateEntryAsset = stateEntryAsset;
            _treeView = rootVisualElement.Q<TreeView>("TreeView");
            _graph.changed.RemoveAllListeners();
            _graph.changed.AddListener(() =>
            {
                _stateTreeItems.Clear();
                GenerateData();
                _treeView.SetRootItems(_stateTreeItems);
                _treeView.RefreshItems();
                _treeView.Rebuild();
            });

            GenerateData();
            _treeView.SetRootItems(_stateTreeItems);
            _treeView.makeItem = MakeItem;
            _treeView.bindItem = BindItem;
            _treeView.destroyItem = DestroyItem;
            _treeView.dataSource = graph.states;
            _treeView.destroyItem = DestroyItem;
            _treeView.reorderable = true;
            _treeView.fixedItemHeight = 32;
            _treeView.canStartDrag += _ => true;
            _treeView.setupDragAndDrop += args => OnSetupDragAndDrop(args, _treeView);
            _treeView.dragAndDropUpdate += args => OnDragAndDropUpdate(args, _treeView);
            _treeView.handleDrop += args => OnHandleDrop(args, _treeView);
            _treeView.itemIndexChanged += (_, _) =>
            {
                Undo.RecordObject(_graph, "Reorder states");
                Save();
            };
            
            _treeView.Rebuild();
        }


        private VisualElement MakeItem()
        {
            return _stateEntryAsset.Instantiate();
        }

        private void BindItem(VisualElement element, int index)
        {
            var data = _treeView.GetItemDataForIndex<State>(index);
            var playerView = element.Q<StateElement>();
            playerView.Bind(data, _graph);
            playerView.id = index;
        }

        private static void DestroyItem(VisualElement element)
        {
            var playerView = element.Q<StateElement>();
            playerView.Reset();
        }

        private static StartDragArgs OnSetupDragAndDrop(SetupDragAndDropArgs args, BaseVerticalCollectionView source)
        {
            var playerView = args.draggedElement.Q<StateElement>();
            if (playerView == null)
            {
                return args.startDragArgs;
            }

            var startDragArgs = new StartDragArgs(args.startDragArgs.title, DragVisualMode.Move);
            startDragArgs.SetGenericData(SourceKey, source);
            var hasSelection = args.selectedIds.Any();

            startDragArgs.SetGenericData(DraggedItemsKey, hasSelection ? args.selectedIds : new List<int> { playerView.id });
            return startDragArgs;
        }

        private DragVisualMode OnDragAndDropUpdate(HandleDragAndDropArgs args, BaseVerticalCollectionView destination, bool isLobby = false)
        {
            var source = args.dragAndDropData.GetGenericData(SourceKey);
            if (source == destination)
            {
                return DragVisualMode.None;
            }

            if (!isLobby && destination.itemsSource.Count >= 3)
            {
                return DragVisualMode.Rejected;
            }
            
            return DragVisualMode.Move;
        }

        private DragVisualMode OnHandleDrop(HandleDragAndDropArgs args, BaseVerticalCollectionView destination, bool isLobby = false)
        {
            if (args.dragAndDropData.entityIds != null)
            {
                var objectsToString = string.Empty;
                foreach (var id in args.dragAndDropData.entityIds)
                {
                    var obj = UnityEditor.EditorUtility.EntityIdToObject(id);
                    var name = obj ? obj.name : "";
                    objectsToString += $"{name}, ";
                }

                if (!string.IsNullOrEmpty(objectsToString))
                {
                    Debug.Log($"That was {objectsToString}");
                    return DragVisualMode.Move;
                }
            }

            if (args.dragAndDropData.GetGenericData(DraggedItemsKey) is not List<int> draggedIds)
            {
                throw new ArgumentNullException($"Indices are null.");
            }

            if (args.dragAndDropData.GetGenericData(SourceKey) is not BaseVerticalCollectionView source)
            {
                throw new ArgumentNullException($"Source is null.");
            }

            // Let default reordering happen.
            if (source == destination)
            {
                return DragVisualMode.None;
            }

            // Be coherent with the dragAndDropUpdate condition.
            if (!isLobby && destination.itemsSource.Count >= 3)
            {
                return DragVisualMode.Rejected;
            }

            var treeViewSource = source as BaseTreeView;

            // ********************************************************
            // Add items first, from item indices in the source.
            // ********************************************************

            // Gather ids from dragged indices
            var ids = new List<int>();

            foreach (var id in draggedIds)
            {
                ids.Add(id);
            }

            // Special TreeView case, we need to gather children or selected indices.
            if (treeViewSource != null)
            {
                GatherChildrenIds(ids, treeViewSource);
            }

            if (destination is BaseTreeView treeView)
            {
                foreach (var data in ids.Select(id => (State)source.viewController.GetItemForId(id)))
                {
                    treeView.AddItem(new TreeViewItemData<State>(0, data), args.parentId, args.childIndex);
                }
                treeView.Rebuild();
            }
            else if (destination.viewController is BaseListViewController destinationListViewController)
            {
                for (var i = ids.Count - 1; i >= 0; i--)
                {
                    var id = ids[i];
                    var data = (State)source.viewController.GetItemForId(id);
                    destinationListViewController.itemsSource.Insert(args.insertAtIndex, data);
                }
            }
            else
            {
                throw new ArgumentException("Unhandled destination.");
            }

            // Then remove from the source.
            if (source is BaseTreeView sourceTreeView)
            {
                foreach (var id in draggedIds)
                {
                    var data = (State)source.viewController.GetItemForId(id);
                    sourceTreeView.viewController.TryRemoveItem(data.id, false);
                }

                sourceTreeView.Rebuild();
                sourceTreeView.RefreshItems();
            }
            else if (source.viewController is BaseListViewController sourceListViewController)
            {
                sourceListViewController.RemoveItems(draggedIds);
            }
            else
            {
                throw new ArgumentException("Unhandled source.");
            }

            foreach (var id in ids)
            {
                var index = destination.viewController.GetIndexForId(id);
                destination.AddToSelection(index);
            }
            source.ClearSelection();
            destination.RefreshItems();
            LogTeamSizes();
            return DragVisualMode.Move;
        }

        void LogTeamSizes()
        {
            Debug.Log($"Red: {_treeView.viewController.GetItemsCount()} / 3");
        }

        private void GenerateData()
        {
            _usedIndices.Clear();

            foreach (var rootState in _graph.rootStates)
            {
                if (rootState < 0 || rootState >= _graph.states.Count)
                {
                    continue;
                }
                
                if (GenerateData(rootState, out var data))
                {
                    _stateTreeItems.Add(data);
                }
            }
        }

        private bool GenerateData(int stateIndex, out TreeViewItemData<State> data, int stackDepth = 0)
        {
            if (stackDepth > MaxDepth)
            {
                Debug.LogWarning("[StateTree] Graph importer went beyond the maximum tree depth");
                data = default;
                return false;
            }
            
            if (_usedIndices.Contains(stateIndex))
            {
                Debug.LogWarning("[StateTree] Graph importer detected a circular reference.");
                data = default;
                return false;
            }

            if (stateIndex < 0 || stateIndex >= _graph.states.Count)
            {
                data = default;
                return false;
            }
            
            _usedIndices.Add(stateIndex);
            var state = _graph.GetState(stateIndex);
            if (state.childStates.Count <= 0)
            {
                data = new TreeViewItemData<State>(stateIndex, state);
                return true;
            }
            
            var subData = new List<TreeViewItemData<State>>();
            foreach (var childStateIndex in state.childStates)
            {
                /*if (GenerateData(childStateIndex, out var child, stackDepth + 1))
                {
                    subData.Add(child);
                }*/
            }
            data = new TreeViewItemData<State>(stateIndex, state, subData);
            return true;

        }

        static void GatherChildrenIds(List<int> ids, BaseTreeView treeView)
        {
            for (var i = 0; i < ids.Count; i++)
            {
                var id = ids[i];
                var childrenIds = treeView.viewController.GetChildrenIds(id);
                foreach (var childId in childrenIds)
                {
                    ids.Insert(i + 1, childId);
                    i++;
                }
            }
        }

        public void Save()
        {
            var oldStates = new List<State>(_graph.states);
            _graph.states.Clear();
            for (var i = 0; i < oldStates.Count; ++i)
            {
                _graph.states.Add(null);
            }
            
            var processedStates = new List<int>();
            var saveQueue = new Queue<int>();
            _graph.rootStates = _treeView.GetRootIds().ToList();
            foreach (var stateData in _graph.rootStates)
            {
                saveQueue.Enqueue(stateData);
                var state = oldStates[stateData];
                var newChildIndices = _treeView.GetChildrenIdsForIndex(stateData).ToList();
                // state.childStates = newChildIndices;
            }

            while (saveQueue.Count > 0)
            {
                var index = saveQueue.Dequeue();
                _graph.states.Insert(index, oldStates[index]);
                processedStates.Add(index);
                var state = _graph.GetState(index);
                var childIndices = _treeView.GetChildrenIdsForIndex(index).ToList();
                foreach (var childIndex in childIndices.Where(childIndex => !processedStates.Contains(childIndex)))
                {
                    saveQueue.Enqueue(childIndex);
                }
                // state.childStates = childIndices;
            }
            _graph.states.RemoveAll(state => state == null);
        }

        public void AddState()
        {
            var state = new State
            {
                name = "New State"
            };
            Undo.RecordObject(_graph, "Add state");
            _graph.states.Add(state);
            _graph.rootStates.Add(_graph.states.Count - 1);
        }
    }
}