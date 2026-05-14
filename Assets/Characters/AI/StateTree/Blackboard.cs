using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace FoxShooter.Characters.AI.StateTree
{
    [Serializable]
    public class Blackboard : ISerializationCallbackReceiver
    {
        [SerializeField] private List<BlackboardVariable> _serializedVariables = new();
        public readonly Dictionary<string, BlackboardVariable> variables = new();
        
        // Maybe switch from UnityEvent for better support on other platforms
        [DoNotSerialize][HideInInspector] public UnityEvent<BlackboardVariable> onVariableAdded = new();
        [DoNotSerialize][HideInInspector] public UnityEvent<BlackboardVariable> onVariableRemoved = new();

        public void AddVariable<T>(string name, T defaultValue = default)
        {
            var newVariable = new BlackboardVariable<T>(name, defaultValue);
            if (variables.TryAdd(name, newVariable))
            {
                onVariableAdded.Invoke(newVariable);
            }
        }

        public bool TryGetVariable<T>(string name, out BlackboardVariable<T> result)
        {
            if (variables.TryGetValue(name, out var value))
            {
                result = value as BlackboardVariable<T>;
                if (result != null)
                {
                    return true;
                }
                Debug.LogError($"[Blackboard] attempt to get variable '{name}' failed. Type mismatch. Expected type {typeof(T)}");
                return false;
            }

            Debug.LogError($"[Blackboard] Couldn't find blackboard variable with key '{name}'");
            result = null;
            return false;
        }

        public bool RemoveVariable(string name)
        {
            if (!variables.Remove(name, out var variableToDelete))
            {
                return false;
            }
            
            onVariableRemoved.Invoke(variableToDelete);
            return true;
        }

        public bool RenameVariable(string oldName, string newName)
        {
            if (oldName == newName)
            {
                return false;
            }
            
            if (variables.ContainsKey(newName))
            {
                return false;
            }
            
            if (!variables.Remove(oldName, out var blackboardVariable))
            {
                return false;
            }

            blackboardVariable.name = newName;
            variables.Add(newName, blackboardVariable);
            return true;
        }

        public void OnBeforeSerialize()
        {
            _serializedVariables = variables.Values.ToList();
        }

        public void OnAfterDeserialize()
        {
            foreach (var variable in _serializedVariables)
            {
                variables.Add(variable.name, variable);
            }
            _serializedVariables.Clear();
        }
    }
}