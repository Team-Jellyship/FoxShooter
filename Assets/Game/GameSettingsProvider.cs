using UnityEditor;
using UnityEngine.UIElements;

namespace FoxShooter.Game
{
    public class GameSettingsProvider : SettingsProvider
    {
        private SerializedObject _settings;

        private GameSettingsProvider(string path, SettingsScope scope = SettingsScope.Project)
            :base(path, scope)
        {}

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = GameSettings.GetSerializedSettings();
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            return new GameSettingsProvider("Project/Game Settings", SettingsScope.Project);
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.PropertyField(_settings.FindProperty("transitions"));
            EditorGUILayout.PropertyField(_settings.FindProperty("effectList"));
            _settings.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}