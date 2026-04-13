using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree
{
    public class StateTreeGraph : ScriptableObject
    {
        // List of states, ordered
        // index should not change between import
        private readonly List<State> _states;

        public State GetState(int index)
        {
            return _states[index];
        }
    }
}