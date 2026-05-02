using System;

namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    [Serializable]
    public class Task
    {
        public virtual TaskStatus Enter()
        {
            return TaskStatus.Succeeded;
        }

        public virtual TaskStatus Update()
        {
            return TaskStatus.Active;
        }
    }
}