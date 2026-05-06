using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;

namespace FoxShooter.Characters.AI.StateTree
{
    public class Tree
    {
        public Blackboard blackboard;
        public State root = new State
        {
            name = "Root"
        };

        /**
         * The currently active states, in order from
         * root to child
         */
        private readonly List<State> _activeStates = new();

        public Tree()
        {
            _activeStates.Add(root);
        }
        
        public void Start()
        {
            CompleteTransition(root);
        }
        
        /**
         * <summary>
         * Update each active state, in order from
         * child to root, aka the inverse of the active
         * states list. Will cause transitions if
         * necessary
         * </summary>
         */
        public void Update(float time)
        {
            var treeContext = new TreeContext
            {
                blackboard = blackboard
            };
            
            for (var i = _activeStates.Count - 1; i >= 0; --i)
            {
                var state = _activeStates[i];
                var result = state.Update(treeContext, time);
                if (result == null) { continue; }
                CompleteTransition(result);
                return;
            }
        }

        /**
         * <summary>
         * Transition to one state from the current state, following
         * all interruptions. This may not necessarily result in transitioning
         * to the desired state, if an interrupt happens.
         * </summary>
         *
         * <param name="state">
         * State to transition towards
         * </param>
         */
        private void CompleteTransition(State state)
        {
            while (state != null)
            {
                state = TransitionTo(state);
            }
        }

        /**
         * <summary>
         * Transition to one state from the current state, first ascending
         * the tree and exiting each state, then descending and entering where necessary.
         * When transitioning from sibling nodes, parents will NOT be entered or exited.
         * This process can be canceled if, when entering a state, that state returns
         * a failure or success.
         * </summary>
         * 
         * <param name="state">
         * State to transition to. Does not currently check if states are in the same tree.
         * </param>
         *
         * <returns>
         * Next state to transition to, if entering a state in the transition path was
         * interrupted, by either a success or failure
         * </returns>
         */
        private State TransitionTo(State state)
        {
            var newActiveStates = GetHierarchy(state);
            var context = new TreeContext();
            
            // Ascend the tree (child -> parent) until hitting a common ancestor
            var i = _activeStates.Count - 1;
        
            for (; i >= 0; --i)
            {
                if (i >= newActiveStates.Count)
                {
                    continue;
                }
                if (_activeStates[i] == newActiveStates[i])
                {
                    break;
                }
                _activeStates[i].Exit();
                _activeStates.RemoveAt(i);
            }


            // Except for entering a parent state directly...
            if (i == newActiveStates.Count - 1)
            {
                _activeStates.RemoveAt(i);
                newActiveStates[i].Exit();
            }
            // ...current index would be the common ancestor, so we don't need to enter that
            else
            {
                ++i;
            }
            for (; i < newActiveStates.Count; ++i)
            {
                var result = newActiveStates[i].Enter(context);
                _activeStates.Add(newActiveStates[i]);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /**
         * <summary>
         * Ascend the tree to the root from a given State
         * </summary>
         * 
         * <param name="state">
         * State to start at
         * </param>
         * 
         * <returns>
         * List of states, in order from State parameter to
         * root state
         * </returns>
         */
        private static List<State> GetHierarchy(State state)
        {
            var result = new List<State>();
            for (var parent = state; parent != null; parent = parent.parent)
            {
                result.Insert(0, parent);
            }
            return result;
        }
    }
}