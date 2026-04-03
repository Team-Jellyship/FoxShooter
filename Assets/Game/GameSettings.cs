using FoxShooter.Game.GamemodeGraph.Runtime;
using UnityEditor;
using UnityEngine;

namespace FoxShooter.Game
{
    public class GameSettings : ScriptableObject
    {
        public const string SettingsFileName = "GameSettings";
        public const string SettingsDefaultPath = "Assets/Resources/GameSettings.asset";

        [SerializeField] public GamemodeTransitionManager transitions;

        public static SerializedObject GetSerializedSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<GameSettings>(SettingsDefaultPath);
            if (settings != null)
            {
                return new SerializedObject(settings);
            }
            
            settings = CreateInstance<GameSettings>();
            AssetDatabase.CreateAsset(settings, SettingsDefaultPath);
            AssetDatabase.SaveAssets();

            return new SerializedObject(settings);
        }
    }
}