using System.Collections.Generic;

namespace FoxShooter.Characters.AI.StateTree
{
    public class Blackboard
    {
        public readonly Dictionary<string, BlackboardVariable> variables = new();

        public void AddVariable<T>(string name)
        {
            variables.Add(name, new BlackboardVariable<T>(name));
        }
        public void AddVariable<T>(string name, T defaultValue)
        {
            variables.Add(name, new BlackboardVariable<T>(name, defaultValue));
        }

        public bool TryGetVariable<T>(string name, out BlackboardVariable<T> result)
        {
            if (variables.TryGetValue(name, out var value))
            {
                result = value as BlackboardVariable<T>;
                return result == null;
            }

            result = null;
            return false;
        }
    }
}