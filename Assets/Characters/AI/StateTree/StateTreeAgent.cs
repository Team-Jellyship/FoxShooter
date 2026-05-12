using System.Collections.Generic;
using FoxShooter.Characters.AI.StateTree.Graph;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree
{
    public class StateTreeAgent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] public StateTreeGraph graph;
        [SerializeReference, HideInInspector] private List<BlackboardVariable> variables;
        
        public Blackboard blackboard;

        private Tree _tree;
        private State _state1;
        private State _state2;

        public StateTreeAgent()
        {
            blackboard = new Blackboard();
            blackboard.AddVariable<float>("time");
            blackboard.AddVariable<float>("time2");

            /*tree = new Tree
            {
                root = new State
                {
                    name = "root"
                }
            };

            _state1 = new State
            {
                name = "state1"
            };

            _state2 = new State
            {
                name = "state2",
                successState = tree.root
            };
            tree.root.AddChild(_state1);
            tree.root.AddChild(_state2);
            tree.root.successState = _state1;
            _state1.successState = _state2;
            tree.blackboard = blackboard;*/
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
            /*var testTask = new TestTask();
            testTask.timeLimit.Set(new BlackboardReference<float>(blackboard, "time"));
            _state1.AddTask(testTask);

            var testTask2 = new TestTask();
            testTask2.timeLimit.Set(new BlackboardReference<float>(blackboard, "time2"));
            _state2.AddTask(testTask2);*/

            _tree = graph.GenerateTree();
            _tree.Start();
        }

        private void Update()
        {
            _tree.Update(Time.deltaTime);
        }
    }
}