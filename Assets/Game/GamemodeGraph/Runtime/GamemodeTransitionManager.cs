using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public class GamemodeTransitionManager : ScriptableObject
    {
        // Just a list for visibility
        public readonly List<string> gamemodes = new();
        public readonly Dictionary<Tuple<Gamemode, GamemodeTransitionFlag>, Gamemode> transitionTable = new();
        public Gamemode startingGameMode;
        public Gamemode currentGamemode { private set; get; }
        
        public void AddTransition(Gamemode startingMode, GamemodeTransitionFlag flag, Gamemode endingMode)
        {
            transitionTable.Add(new Tuple<Gamemode, GamemodeTransitionFlag>(startingMode, flag), endingMode);
        }

        public void Startup()
        {
            currentGamemode = startingGameMode;
            Debug.Log($"[GamemodeTransitionManager] Entering mode '{startingGameMode.name}'");
        }
    }
}