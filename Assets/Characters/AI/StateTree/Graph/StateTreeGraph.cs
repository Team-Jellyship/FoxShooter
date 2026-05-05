using System;
using System.Collections.Generic;
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
        [SerializeField] private List<StateNode> nodes;
        [SerializeField] private StateNodeIdentifier rootNode;
        
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
    }
}