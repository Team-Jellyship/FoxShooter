using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace FoxShooter.Characters.AI.StateTree
{
    [Serializable]
    public class State
    {
        [SerializeReference] public List<int> childStates;
        [SerializeReference] public List<Task> childTasks;
        
        [SerializeReference] public int successTask;
        [SerializeReference] public int cancelTask;
        
        private int _childStateIndex;
        
        // returns true if this state is ended, and the parent should progress to the next one
        public bool Enter(TreeContext context)
        {
            var shouldExit = false;

            if (childStates.Count > 0)
            {
                _childStateIndex = 0;
                var childState = context.GetState(_childStateIndex);
                while (childState.Enter(context))
                {
                    ++_childStateIndex;
                    childState = context.GetState(_childStateIndex);
                }
            }
            
            foreach (var task in childTasks)
            {
                var result = task.Enter();
                if (result == TaskStatus.Completed)
                {
                    shouldExit = true;
                }
            }

            return shouldExit;
        }

        public bool Update(TreeContext context)
        {
            var shouldExit = false;
            
            context.GetState(_childStateIndex).Update(context);
            
            foreach (var task in childTasks)
            {
                var result = task.Update();
                if (result == TaskStatus.Completed)
                {
                    shouldExit = true;
                }
            }

            return shouldExit;
        }

        public void Exit()
        {
            foreach (var task in childTasks)
            {
                Exit();
            }
        }
    }
}