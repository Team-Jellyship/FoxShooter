using System;
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

        public int levelMaxCombo;
        public LevelDefinition currentLevel;
        public LevelDefinition nextLevel;
        public StatusEffectList statusEffects;
        public CharacterStats character;
        public bool paused;

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
            if (_gameSettings.loadGameModeGraphOnStart)
            {
                _transitionTable.Startup(_gameSettings.transitions);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            statusEffects = _gameSettings.effectList;
            _gameSettings.scoreImporter.LoadScores();
        }

        // Spawns a new menu object 
        public void LoadMenu(GameObject menu)
        {
            UnloadMenu();
            // SceneManager.LoadScene(_gameSettings.defaultScene.BuildIndex);
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

        public void SaveCurrentCombo()
        {
            currentLevel.highScore = MathF.Max(currentLevel.highScore, levelMaxCombo);
            Save();
        }

        public void Save()
        {
            _gameSettings.scoreImporter.SaveScores();
        }

        public void RestartCurrentLevel()
        {
            // This isn't an ideal way, but it works okay for now
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SetScore(0.0f);
        }

        public void DestroyPlayer()
        {
            if (character && character.gameObject)
            {
                Destroy(character.gameObject);
            }
            else
            {
                Debug.LogWarning("[Game] Attempted to destroy player, but player was null.");
            }
        }

        private void SetScore(float newScore)
        {
            score = newScore;
            scoreChanged.Invoke(score);
        }
    }
}