using System;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    [Serializable]
    public class TaskVariableNode
    {
        [SerializeField] public string name;

        [SerializeReference] public TaskVariable variable;
    }
}