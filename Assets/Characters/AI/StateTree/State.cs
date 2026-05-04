using System;
using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.Tasks;
using JetBrains.Annotations;
using UnityEngine;

// Query is going to be too slow for this. There's no need to deal with LINQ allocations

// ReSharper disable ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace FoxShooter.Characters.AI.StateTree
{
    public class State
    {
        /// A helpful identifier for visualizing in a graph and debug
        [SerializeField] public string name;
        
        /**
         * Child states of this state. These aren't necessarily entered
         * when this state is, but this state will be active whenever any
         * of its children are active.
         */
        [SerializeReference] public readonly List<State> childStates = new();
        
        /**
         * Child tasks will be active whenever this state is active. If a child
         * task returns TaskStatus.Succeeded or TaskStatus.Failed, this will cause
         * the state to exit.
         */
        [SerializeReference] public readonly List<Task> childTasks = new();

        /**
         * State that this is a child of. Can be null, if this state is a top
         * level state.
         */
        public State parent;
        
        /**
         * State to enter on a success in either Enter or Update
         */
        public State successState;
        
        /**
         * State to enter on a failure in either Enter or Update
         */
        public State cancelState;
        
        public int id { get; private set; }
        
        
        /**
         * <summary>
         * Enter this state, entering child tasks in order.
         * </summary>
         * 
         * <param name="context">
         * Tree context object, to pass to tasks
         * </param>
         * 
         * <returns>
         * Next state to enter, if any child task succeeds or fails.
         * Failure will always take precedence over success, in the event
         * that multiple tasks report them.
         * If all child tasks are in progress, returns null
         * </returns>
         */
        
        [CanBeNull]
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

        /**
         * <summary>
         * Update this state, updating child tasks in order.
         * </summary>
         * 
         * <param name="context">
         * Tree context object, to pass to tasks
         * </param>
         * <param name="time">
         * How much time has elapsed since the last update
         * </param>
         * 
         * <returns>
         * Next state to enter, if any child task succeeds or fails.
         * Failure will always take precedence over success, in the event
         * that multiple tasks report them.
         * If all child tasks are in progress, returns null
         * </returns>
         */
        public State Update(TreeContext context, float time)
        {
            var succeeded = false;
            var canceled = false;
            
            foreach (var task in childTasks)
            {
                var result = task.Update(context, time);
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

        /**
         * <summary>
         * Exit this state, cancelling all child tasks
         * </summary>
         */
        public void Exit()
        {
            foreach (var task in childTasks)
            {
                Exit();
            }
        }
    }
}