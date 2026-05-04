using System;

namespace FoxShooter.Characters.AI.StateTree.Tasks
{
    [Serializable]
    public abstract class Task
    {
        public abstract TaskStatus Enter();

        public abstract TaskStatus Update(TreeContext context, float time);

        public virtual void Exit() {}
    }
}