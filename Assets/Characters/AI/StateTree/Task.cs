using System;

namespace FoxShooter.Characters.AI.StateTree
{
    [Serializable]
    public class Task
    {
        public virtual TaskStatus Enter()
        {
            return TaskStatus.Completed;
        }

        public virtual TaskStatus Update()
        {
            return TaskStatus.Active;
        }
    }
}