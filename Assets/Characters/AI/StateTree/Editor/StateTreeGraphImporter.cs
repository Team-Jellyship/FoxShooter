using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.Graph;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Characters.AI.StateTree.Editor
{
    [ScriptedImporter(1, StateTreeGraph.AssetExtension)]
    public class StateTreeGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var graph = GraphDatabase.LoadGraphForImporter<StateTreeGraph>(ctx.assetPath);
            var stateTree = ScriptableObject.CreateInstance<StateTreeSerialized>();
            var stateNodes = new List<StateNode>();

            var nodesToBuild = new Queue<StateNode>();
            var root = GetRoot(graph);
            nodesToBuild.Enqueue(root);
            while (nodesToBuild.TryDequeue(out var currentNode))
            {
                stateNodes.Add(currentNode);
                currentNode.id = stateNodes.Count;
                foreach (var child in currentNode.GetChildren())
                {
                    nodesToBuild.Enqueue(child);
                }
            }

            foreach (var entry in stateNodes)
            {
                stateTree.states.Add(BuildData(entry));
            }

            stateTree.startingState = root.GetName();
            ctx.AddObjectToAsset("RuntimeData", stateTree);
            ctx.SetMainObject(stateTree);
        }
        
        private static StateNode GetRoot(StateTreeGraph graph)
        {
            foreach (var node in graph.GetNodes())
            {
                if (node is RootNode rootNode)
                {
                    return rootNode.GetNode();
                }
            }

            return null;
        }

        private static StateNodeData BuildData(StateNode node)
        {
            var data = new StateNodeData
            {
                name = node.GetName(),
            };
            foreach (var childNode in node.GetChildren())
            {
                data.childStates.Add(childNode.id);
            }
            var parent = node.GetParent();
            var success = node.GetSucceed();
            var cancel = node.GetCancelled();
            
            if (parent != null)
            {
                data.parent = parent.id;
            }
            if (success != null)
            {
                data.successState = success.id;
            }
            if (cancel != null)
            {
                data.cancelState = cancel.id;
            }
            return data;
        }
    }
}