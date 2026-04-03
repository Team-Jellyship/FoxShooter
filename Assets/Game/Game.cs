using Unity.VisualScripting;
using UnityEngine;

namespace FoxShooter.Game
{
    public class Game : MonoBehaviour
    {
        public static Game instance { get; private set; }

        private GameSettings _gameSettings;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            var gameObject = new GameObject("Game");
            DontDestroyOnLoad(gameObject);
            instance = gameObject.AddComponent<Game>();
        }

        private void Start()
        {
            _gameSettings = Resources.Load<GameSettings>(GameSettings.SettingsFileName);
            _gameSettings.transitions.Startup();
        }
    }
}