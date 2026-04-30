using Eflatun.SceneReference;
using FoxShooter.Game.GamemodeGraph.Runtime;
using FoxShooter.Game.StatusEffects;
using UnityEditor;
using UnityEngine;

namespace FoxShooter.Game
{
    public class GameSettings : ScriptableObject
    {
        public const string SettingsFileName = "GameSettings";
        public const string SettingsDefaultPath = "Assets/Resources/GameSettings.asset";

        [SerializeField] public GamemodeTransitionData transitions;
        [SerializeField] public StatusEffectList effectList;
        [SerializeField] public SceneReference defaultScene;
        [SerializeField] public ScoreImporter scoreImporter;
        [SerializeField] public bool loadGameModeGraphOnStart = true;
        
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