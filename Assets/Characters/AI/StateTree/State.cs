using System;
using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.Tasks;
using UnityEngine;

// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace FoxShooter.Characters.AI.StateTree
{
    public class State
    {
        [SerializeField] public string name;
        [SerializeReference] public List<State> childStates = new();
        [SerializeReference] public List<Task> childTasks = new();

        public State parent;
        public State successState;
        public State cancelState;
        
        public int id { get; private set; }
        
        // returns true if this state is ended, and the parent should progress to the next one
        public State Enter(TreeContext context)
        {
            var cancelled = false;
            var succeeded = false;
            
            foreach (var task in childTasks)
            {
                var result = task.Enter();
                switch (result)
                {
                    case TaskStatus.Succeeded:
                        succeeded = true;
                        continue;

                    case TaskStatus.Cancelled:
                        cancelled = true;
                        continue;

                    default:
                    case TaskStatus.Active:
                        break;
                }
            }

            return cancelled ? cancelState : succeeded ? successState : null;
        }

        public State Update(TreeContext context)
        {
            var succeeded = false;
            var canceled = false;
            
            foreach (var task in childTasks)
            {
                var result = task.Update();
                switch (result)
                {
                    case TaskStatus.Succeeded:
                        succeeded = true;
                        break;

                    case TaskStatus.Cancelled:
                        canceled = true;
                        break;

                    case TaskStatus.Active:
                    default:
                        break;
                }
            }

            return canceled ? cancelState : succeeded ? successState : null;
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