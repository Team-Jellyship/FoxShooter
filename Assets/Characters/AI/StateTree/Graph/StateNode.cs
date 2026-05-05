using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    [Serializable]
    public class StateNode
    {
        [SerializeField] public StateNodeIdentifier id;
        [SerializeField] public List<StateNodeIdentifier> childStates;
        [SerializeField] public List<TaskNode> tasks;
    }
}