using System;
using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.Tasks;
using FoxShooter.Characters.AI.StateTree.UI;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree
{
    public class StateTreeAgent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeReference, HideInInspector] private List<BlackboardVariable> variables;
        
        public Blackboard blackboard;
        public StateTree stateTree;

        private State _state1;
        private State _state2;

        public StateTreeAgent()
        {
            blackboard = new Blackboard();
            blackboard.AddVariable<float>("time");

            stateTree = new StateTree
            {
                _root = new State
                {
                    name = "start"
                }
            };

            _state1 = new State
            {
                name = "state1"
            };

            _state2 = new State
            {
                name = "state2"
            };
            stateTree._root.AddState(_state1);
            stateTree._root.AddState(_state2);
            stateTree._root.successState = _state1;
            _state1.successState = _state2;
            stateTree.blackboard = blackboard;
        }

        public void OnBeforeSerialize()
        {
            variables = new List<BlackboardVariable>();
            foreach (var variable in blackboard.variables.Values)
            {
                variables.Add(variable);
            }
        }

        public void OnAfterDeserialize()
        {
            blackboard.variables.Clear();
            foreach (var variable in variables)
            {
                blackboard.variables.Add(variable.name, variable);
            }
        }

        private void Start()
        {
            var testTask = new TestTask();
            testTask.timeLimit.Set(new BlackboardReference<float>(blackboard, "time"));
            _state1.childTasks.Add(testTask);
            stateTree.Start();
        }

        private void Update()
        {
            stateTree.Update(Time.deltaTime);
        }
    }
}