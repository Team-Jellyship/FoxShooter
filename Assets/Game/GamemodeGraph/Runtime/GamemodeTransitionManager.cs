using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public struct GamemodeTransitionEntry
    {
        [SerializeReference]
        public Gamemode modeIn;

        [SerializeField]
        public GamemodeTransitionFlag transition;

        [SerializeReference]
        public Gamemode modeOut;
    }
    
    [Serializable]
    public class GamemodeTransitionManager : ScriptableObject
    {
        // This data is serialized from GamemodeGraphImporter
        [SerializeField] public List<Gamemode> gamemodes = new();
        [SerializeField] public List<GamemodeTransitionEntry> transitionEntries = new();
        [SerializeReference] public Gamemode startingGameMode;
        
        public Gamemode currentGamemode { private set; get; }

        private readonly Dictionary<Tuple<Gamemode, GamemodeTransitionFlag>, Gamemode> _transitionTable = new();

        private TimerHandle _transitionTimer;
        
        
        public void AddTransition(Gamemode startingMode, GamemodeTransitionFlag flag, Gamemode endingMode)
        {
            transitionEntries.Add(new GamemodeTransitionEntry
            {
                modeIn = startingMode,
                transition = flag,
                modeOut = endingMode
            });
        }

        public void Startup()
        {
            currentGamemode = startingGameMode;
            LoadTransitions();

            _transitionTimer = TimerManager.instance.CreateTimer(Game.instance, () => Command(GamemodeTransitionFlag.Timeout));
            EnterMode(currentGamemode);
        }

        public void Command(GamemodeTransitionFlag transition)
        {
            if (!_transitionTable.TryGetValue(new Tuple<Gamemode, GamemodeTransitionFlag>(currentGamemode, transition), out var nextMode))
            {
                return;
            }
            
            currentGamemode.Exit();
            EnterMode(nextMode);
        }

        private void LoadTransitions()
        {
            _transitionTable.Clear();
            
            foreach (var entry in transitionEntries)
            {
                _transitionTable.Add(new Tuple<Gamemode, GamemodeTransitionFlag>(entry.modeIn, entry.transition), entry.modeOut);    
            }
        }

        private void EnterMode(Gamemode mode)
        {
            Debug.Log($"[GamemodeTransitionManager] Entering mode '{mode.name}'");
            currentGamemode = mode;
            mode.Enter();
            if (mode.time > 0.0f)
            {
                _transitionTimer.Start(currentGamemode.time);
            }
        }
    }
}