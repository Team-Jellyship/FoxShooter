using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    [Serializable]
    public class StateNode
    {
        [SerializeField] public string name;
        [SerializeField] public StateNodeIdentifier id;
        [SerializeField] public List<StateNodeIdentifier> childStates;
        [SerializeField] public List<TaskNode> tasks;
        [SerializeField] public StateNodeIdentifier success = StateNodeIdentifier.invalid;
        [SerializeField] public StateNodeIdentifier cancel = StateNodeIdentifier.invalid;

        public State GenerateState()
        {
            var state = new State
            {
                name = name
            };

            foreach (var task in tasks)
            {
                state.childTasks.Add(task.GenerateTask());
            }
            return state;
        }

        public static StateNode Serialize(State state, StateNodeIdentifier id, Dictionary<State, StateNodeIdentifier> dictionary)
        {
            var node = new StateNode
            {
                id = id
            };

            foreach (var childState in state.childStates)
            {
                node.childStates.Add(dictionary[childState]);
            }
            
            if (state.successState != null)
            {
                node.success = dictionary[state.successState];
            }
            if (state.cancelState != null)
            {
                node.cancel = dictionary[state.cancelState];
            }

            foreach (var task in state.childTasks)
            {
                node.tasks.Add(TaskNode.Serialize(task));
            }

            return node;
        }
    }
}