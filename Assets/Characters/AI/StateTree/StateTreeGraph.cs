using System;
using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.UI;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;

namespace FoxShooter.Characters.AI.StateTree
{
    [CreateAssetMenu(menuName = "StateTree/Graph", fileName = "ST_Tree")]
    public class StateTreeGraph : ScriptableObject
    {
        // List of states, ordered
        // index should not change between import
        [SerializeField]
        public List<State> states = new();

        public List<int> rootStates = new();

        [OnOpenAsset(OnOpenAssetAttributeMode.Execute)]
        public static bool OpenGameStateWindow(int instanceID)
        {
            if (!EditorWindow.HasOpenInstances<StateTreeGraphEditor>())
            {
                EditorWindow.CreateWindow<StateTreeGraphEditor>();
            }
            else
            {
                EditorWindow.FocusWindowIfItsOpen<StateTreeGraphEditor>();
            }

            var asset = EditorUtility.EntityIdToObject(instanceID) as StateTreeGraph;
            var window = EditorWindow.GetWindow<StateTreeGraphEditor>();
            window.LoadState(asset);
            return true;
        }
        
        public State GetState(int index)
        {
            return states[index];
        }

        public void AddState()
        {
            states.Add(new State());
        }
    }
}