using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    public class TestTask : Task
    {
        [SerializeReference] public TaskVariable<float> timeLimit = new();
        [SerializeReference] public TaskVariable<KinematicCharacter> character = new();

        private float _currentTime;
        
        public override TaskStatus Enter()
        {
            _currentTime = 0.0f;
            return TaskStatus.Active;
        }

        public override TaskStatus Update(TreeContext context, float time)
        {
            _currentTime += time;

            var limit = timeLimit.Get(context);
            
            return _currentTime < limit ? TaskStatus.Active : TaskStatus.Succeeded;
        }
    }
}