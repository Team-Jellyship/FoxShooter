using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

using FoxShooter.Characters.AI.StateTree.UI;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    
    [Serializable]
    [CreateAssetMenu(fileName = "Tree", menuName = "StateTree/Tree")]
    public class StateTreeGraph : ScriptableObject
    {
        [SerializeField] private List<StateNode> nodes = new();
        [SerializeField] private StateNodeIdentifier rootNode = StateNodeIdentifier.invalid;
        
        [OnOpenAsset(1)]
        public static bool OpenAsset(int instanceId, int line)
        {
            var asset = EditorUtility.EntityIdToObject(instanceId) as StateTreeGraph;

            if (asset == null)
            {
                return false;
            }
            
            StateTreeWindowDelegate.Open(asset);
            return true;
        }

        public Tree GenerateTree()
        {
            if (rootNode == StateNodeIdentifier.invalid)
            {
                return null;
            }

            var tree = new Tree();
            var stateDictionary = nodes.ToDictionary(node => node.id, node => node.GenerateState());
            var stateNodeDictionary = nodes.ToDictionary(stateNode => stateNode.id);

            tree.root = stateDictionary[rootNode];
            var pendingStateConnections = new Queue<StateNode>();
            pendingStateConnections.Enqueue(stateNodeDictionary[rootNode]);
            
            while (pendingStateConnections.Count > 0)
            {
                var pendingStateNode = pendingStateConnections.Dequeue();
                var pendingState = stateDictionary[pendingStateNode.id];
                
                foreach (var childStateId in pendingStateNode.childStates)
                {
                    pendingState.childStates.Add(stateDictionary[childStateId]);
                    pendingStateConnections.Enqueue(stateNodeDictionary[childStateId]);
                }

                if (pendingStateNode.success)
                {
                    pendingState.successState = stateDictionary[pendingStateNode.success];
                }
                if (pendingStateNode.cancel)
                {
                    pendingState.cancelState = stateDictionary[pendingStateNode.cancel];
                }
            }

            return tree;
        }

        public static StateTreeGraph SerializeTree(Tree tree)
        {
            var result = CreateInstance<StateTreeGraph>();
            
            var stateDictionary = new Dictionary<State, StateNodeIdentifier>();
            
            // iterate through each state, and add it to the dictionary
            var stateQueue = new Queue<State>();
            stateQueue.Enqueue(tree.root);
            while (stateQueue.Count > 0)
            {
                var currentState = stateQueue.Dequeue();
                stateDictionary.Add(currentState, new StateNodeIdentifier(stateDictionary.Count));
                foreach (var childState in currentState.childStates)
                {
                    stateQueue.Enqueue(childState);
                }
            }

            foreach (var pair in stateDictionary)
            {
                result.nodes.Add(StateNode.Serialize(pair.Key, pair.Value, stateDictionary));
            }
            result.rootNode = stateDictionary[tree.root];
            
            return result;
        }
    }
}