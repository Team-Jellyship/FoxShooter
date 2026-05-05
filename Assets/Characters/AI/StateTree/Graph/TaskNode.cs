using System;
using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    [Serializable]
    public class TaskNode
    {
        [SerializeReference] public Type taskClassType;
        [SerializeReference] public List<TaskVariable> variables;
    }
}