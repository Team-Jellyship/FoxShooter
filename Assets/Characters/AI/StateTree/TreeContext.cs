using JetBrains.Annotations;

namespace FoxShooter.Characters.AI.StateTree
{
    public class TreeContext
    {
        public StateTreeGraphOld currentGraph;

        public State GetState(int index)
        {
            return currentGraph.GetState(index);
        }
    }
}