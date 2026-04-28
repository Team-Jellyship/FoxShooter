using Eflatun.SceneReference;
using FoxShooter.Characters;
using FoxShooter.Game.GamemodeGraph.Runtime;
using FoxShooter.Game.StatusEffects;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

namespace FoxShooter.Game
{
    public class Game : MonoBehaviour
    {
        [field: SerializeField] public float score { get; private set; }
        [field: SerializeField] public CharacterStats player { get; private set; }
        
        public UnityEvent<float> scoreChanged = new();
        
        public static Game instance { get; private set; }

        public StatusEffectList statusEffects;

        private GameSettings _gameSettings;
        private readonly GameTransitionTable _transitionTable = new();
        private GameObject _menuAttachmentPoint;
        private EventSystem _eventSystem;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void OnAfterAssembliesLoaded()
        {
            var gameObject = new GameObject("Game");
            DontDestroyOnLoad(gameObject);
            instance = gameObject.AddComponent<Game>();
        }

        private void Start()
        {
            _eventSystem = gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<InputSystemUIInputModule>();
            _menuAttachmentPoint = new GameObject("menu");
            DontDestroyOnLoad(_menuAttachmentPoint);
            _gameSettings = Resources.Load<GameSettings>(GameSettings.SettingsFileName);
            _transitionTable.Startup(_gameSettings.transitions);
            statusEffects = _gameSettings.effectList;
            // Cursor.lockState = CursorLockMode.Locked;
        }

        // Spawns a new menu object 
        public void LoadMenu(GameObject menu)
        {
            UnloadMenu();
            SceneManager.LoadScene(_gameSettings.defaultScene.BuildIndex);
            Instantiate(menu, _menuAttachmentPoint.transform, false);
        }

        public void UnloadMenu()
        {
            _menuAttachmentPoint.RemoveAllChildren();
        }

        public void Command(GamemodeTransitionFlag flag)
        {
            _transitionTable.Command(flag);
        }

        public void Score(float points)
        {
            if (points < 0.0f)
            {
                return;
            }

            score += points;
            scoreChanged.Invoke(score);
        }

        public void RestartCurrentLevel()
        {
            // This isn't an ideal way, but it works okay for now
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SetScore(0.0f);
        }

        private void SetScore(float newScore)
        {
            score = newScore;
            scoreChanged.Invoke(score);
        }

        public void SetNextLevel(SceneReference level)
        {
        }
    }
}