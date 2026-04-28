using UnityEngine;
using UnityEngine.AI;

namespace Characters.AI.Polestar
{
    public static class PolestarNavigation
    {
        public static bool IsLocationInNavMesh(Vector3 location)
        {
            return NavMesh.SamplePosition(location, out var hit, 0.1f, 1 << NavMesh.GetAreaFromName("Walkable"));
        }
    }
}