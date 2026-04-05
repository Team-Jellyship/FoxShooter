using FoxShooter.Game.GamemodeGraph.Runtime;
using FoxShooter.Scripts;
using UnityEngine;

namespace FoxShooter.Game
{
    public class Game : MonoBehaviour
    {
        public static Game instance { get; private set; }

        private GameSettings _gameSettings;
        private readonly GameTransitionTable _transitionTable = new();
        private GameObject _menuAttachmentPoint;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            var gameObject = new GameObject("Game");
            DontDestroyOnLoad(gameObject);
            instance = gameObject.AddComponent<Game>();
        }

        private void Start()
        {
            _menuAttachmentPoint = new GameObject("menu");
            DontDestroyOnLoad(_menuAttachmentPoint);
            _gameSettings = Resources.Load<GameSettings>(GameSettings.SettingsFileName);
            _transitionTable.Startup(_gameSettings.transitions);
        }

        // Spawns a new menu object 
        public void LoadMenu(GameObject menu)
        {
            UnloadMenu();
            Instantiate(menu, _menuAttachmentPoint.transform, false);
        }

        public void UnloadMenu()
        {
            _menuAttachmentPoint.RemoveAllChildren();
        }
    }
}