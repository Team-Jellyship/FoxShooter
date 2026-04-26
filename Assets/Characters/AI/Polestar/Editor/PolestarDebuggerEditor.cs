using UnityEditor;
using UnityEngine;

namespace Characters.AI.Polestar.Editor
{
    [CustomEditor(typeof(PolestarDebugger))]
    public class PolestarDebuggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var polestarDebugger = target as PolestarDebugger;
            if (GUILayout.Button("Preview"))
            {
                polestarDebugger?.RunQuery();
            }
            if (GUILayout.Button("Clear"))
            {
                polestarDebugger?.ClearQuery();
            }
        }
    }
}