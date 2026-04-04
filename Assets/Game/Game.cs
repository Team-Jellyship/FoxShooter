using FoxShooter.Game.GamemodeGraph.Runtime;
using Unity.VisualScripting;
using UnityEngine;

namespace FoxShooter.Game
{
    public class Game : MonoBehaviour
    {
        public static Game instance { get; private set; }

        private GameSettings _gameSettings;
        private readonly GameTransitionTable _transitionTable = new();
        
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
            _transitionTable.Startup(_gameSettings.transitions);
        }
    }
}