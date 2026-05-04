namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    public class TestTask : Task
    {
        public TaskVariable<float> timeLimit;

        private float _currentTime;
        
        public override TaskStatus Enter()
        {
            return TaskStatus.Active;
        }

        public override TaskStatus Update(TreeContext context, float time)
        {
            _currentTime += time;

            return time >= timeLimit.Get(context) ? TaskStatus.Active : TaskStatus.Succeeded;
        }
    }
}