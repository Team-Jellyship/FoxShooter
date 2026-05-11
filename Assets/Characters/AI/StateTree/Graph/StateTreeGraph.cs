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
        [SerializeField] /*[HideInInspector]*/ public List<StateNode> nodes = new();
        [SerializeField] /*[HideInInspector]*/ public StateNodeIdentifier rootNode = StateNodeIdentifier.invalid;
        [SerializeField] public Blackboard blackboard;

        public StateTreeGraph()
        {
            blackboard = new Blackboard();
            blackboard.AddVariable("test", "default");
        }
        
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
            
            Debug.Log("[StateTreeGraph] Generating tree from nodes...");

            var tree = new Tree();
            var stateDictionary = nodes.ToDictionary(node => node.id, node => node.GenerateState());
            var stateNodeDictionary = nodes.ToDictionary(stateNode => stateNode.id);

            if (!stateDictionary.TryGetValue(rootNode, out var root))
            {
                Debug.LogError("[StateTreeGraph] Failed to generate tree. The root node was missing.");
                return null;
            }

            tree.root = root;
            var pendingStateConnections = new Queue<StateNode>();
            pendingStateConnections.Enqueue(stateNodeDictionary[rootNode]);
            
            while (pendingStateConnections.Count > 0)
            {
                var pendingStateNode = pendingStateConnections.Dequeue();
                var pendingState = stateDictionary[pendingStateNode.id];
                
                foreach (var childStateId in pendingStateNode.childStates)
                {
                    if (!stateDictionary.TryGetValue(childStateId, out var childState))
                    {
                        Debug.LogError("[StateTreeGraph] A child state was missing. It's possible the file was corrupted.");
                        continue;
                    }
                    pendingState.AddChild(childState);
                    pendingStateConnections.Enqueue(stateNodeDictionary[childStateId]);
                    
                }

                if (pendingStateNode.success && stateDictionary.TryGetValue(pendingStateNode.success, out var success))
                {
                    pendingState.successState = success;
                }
                if (pendingStateNode.cancel && stateDictionary.TryGetValue(pendingStateNode.cancel, out var cancel))
                {
                    pendingState.cancelState = cancel;
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