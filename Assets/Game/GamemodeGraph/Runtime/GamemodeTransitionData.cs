using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    // We have to store references as some kind of string or id
    [Serializable]
    public struct GamemodeTransitionEntry
    {
        [SerializeReference]
        public string modeIn;

        [SerializeField]
        public GamemodeTransitionFlag transition;

        [SerializeReference]
        public string modeOut;
    }
    
    public class GamemodeTransitionData : ScriptableObject
    {
        // This data is serialized from GamemodeGraphImporter
        [SerializeField] public List<Gamemode> gamemodes;
        [SerializeField] public List<GamemodeTransitionEntry> transitionEntries;
        [SerializeField] public string startingGameModeId;
        
        public void AddTransition(Gamemode startingMode, GamemodeTransitionFlag flag, Gamemode endingMode)
        {
            transitionEntries.Add(new GamemodeTransitionEntry
            {
                modeIn = startingMode.id,
                transition = flag,
                modeOut = endingMode.id
            });
        }

        public Gamemode FindById(string id)
        {
            return gamemodes.Find(gamemode => gamemode.id == id);
        }
    }
}