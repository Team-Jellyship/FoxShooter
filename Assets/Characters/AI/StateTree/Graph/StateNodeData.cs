using System;
using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    
    // Data representation of StateTree's state, for safer serialization
    [Serializable]
    public class StateNodeData
    {
        [SerializeField] public string name;
        [SerializeReference] public List<int> childStates = new();
        [SerializeReference] public List<Task> childTasks = new();

        public int parent;
        public int successState;
        public int cancelState;
    }
}