using System.Collections.Generic;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    public class StateTreeSerialized : ScriptableObject
    {
        public List<StateNodeData> states = new();

        public string startingState;
    }
}