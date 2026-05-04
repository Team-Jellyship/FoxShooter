namespace FoxShooter.Characters.AI.StateTree
{
    public class TreeContext
    {
        public StateTreeGraphOld currentGraph;

        public Blackboard blackboard;
        
        public State GetState(int index)
        {
            return currentGraph.GetState(index);
        }
    }
}