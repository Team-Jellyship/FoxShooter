using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoxShooter.Characters.AI.StateTree
{
    public class StateTreeAgent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeReference, HideInInspector] private List<BlackboardVariable> variables;
        
        public Blackboard blackboard;

        public StateTreeAgent()
        {
            blackboard = new Blackboard();
            blackboard.AddVariable<GameObject>("Test");
            blackboard.AddVariable<bool>("Test2");
            blackboard.AddVariable<CharacterStats>("Test3");
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
    }
}