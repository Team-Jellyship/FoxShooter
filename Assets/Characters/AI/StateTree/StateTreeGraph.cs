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
        public UnityEvent changed;
        
        // List of states, ordered
        // index should not change between import
        [SerializeField]
        public List<State> states = new();

        public List<int> rootStates = new();

        private void OnValidate()
        {
            changed.Invoke();
        }

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

        public bool TryGetState(int index, out State state)
        {
            if (index < 0 || index >= states.Count)
            {
                state = null;
                return false;
            }

            state = states[index];
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