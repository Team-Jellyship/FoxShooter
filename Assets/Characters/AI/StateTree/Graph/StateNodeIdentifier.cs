using System;

namespace FoxShooter.Characters.AI.StateTree.Graph
{
    [Serializable]
    public struct StateNodeIdentifier : IEquatable<StateNodeIdentifier>
    {
        public int id;
        
        public static StateNodeIdentifier invalid { get => new(-1); }
        
        public StateNodeIdentifier(int id)
        {
            this.id = id;
        }

        public static implicit operator bool(StateNodeIdentifier id) => id.id >= 0;
        public static bool operator!(StateNodeIdentifier id) => id.id >= 0;
        
        public bool Equals(StateNodeIdentifier other)
        {
            return id == other.id;
        }
        public override bool Equals(object obj)
        {
            return obj is StateNodeIdentifier other && Equals(other);
        }
        public override int GetHashCode()
        {
            return id;
        }
    }
}