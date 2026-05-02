using FoxShooter.Characters.AI.StateTree;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.StateTree.Editor
{
    [CustomEditor(typeof(StateTreeAgent))]
    public class StateTreeAgentEditor : UnityEditor.Editor
    {
        private bool _displayingBlackboard = true;
        
        public override void OnInspectorGUI()
        {
            var agent = serializedObject.targetObject as StateTreeAgent;
            if (agent && agent.blackboard != null)
            {
                DrawBlackboard(agent.blackboard);
            }
        }

        private void DrawBlackboard(Blackboard blackboard)
        {
            _displayingBlackboard = EditorGUILayout.Foldout(_displayingBlackboard, new GUIContent("Blackboard"));
            EditorGUI.indentLevel++;
            if (!_displayingBlackboard) { return; }
            
            foreach (var variable in blackboard.variables.Values)
            {
                DrawBlackboardVariable(variable);
            }
            EditorGUI.indentLevel--;
        }

        private static void DrawBlackboardVariable(BlackboardVariable variable)
        {
            var label = new GUIContent(variable.name, "Blackboard variable");
            EditorGUI.BeginChangeCheck();
            if (variable.type == typeof(float))
            {
                var newValue = EditorGUILayout.FloatField(label, (float)variable.objectData);
                SetBlackboardVariable(variable, newValue);
                return;
            }

            if (variable.type == typeof(bool))
            {
                var newValue = EditorGUILayout.Toggle(label, (bool)variable.objectData);
                SetBlackboardVariable(variable, newValue);
                return;
            }

            if (variable.type == typeof(GameObject))
            {
                var gameObject = (GameObject) variable.objectData;
                var newValue = EditorGUILayout.ObjectField(label, gameObject, variable.type, true);
                SetBlackboardVariable(variable, newValue);
                return;
            }

            if (variable.type.IsSubclassOf(typeof(Object)))
            {
                var obj = (Object)variable.objectData;
                var newValue = EditorGUILayout.ObjectField(label, obj, variable.type, true);
                SetBlackboardVariable(variable, newValue);
            }
        }

        private static void SetBlackboardVariable<T>(BlackboardVariable variable, T value)
        {
            if (EditorGUI.EndChangeCheck())
            {
                variable.objectData = value;
            }
        }
    }
}