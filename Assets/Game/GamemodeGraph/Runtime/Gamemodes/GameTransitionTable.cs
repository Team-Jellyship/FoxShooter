using System;
using System.Collections.Generic;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    public class GameTransitionTable
    {
        private Gamemode _currentGamemode;
        private readonly Dictionary<Tuple<Gamemode, GamemodeTransitionFlag>, Gamemode> _transitionTable = new();

        private TimerHandle _transitionTimer;

        public void Startup(GamemodeTransitionData data)
        {
            _transitionTable.Clear();

            foreach (var entry in data.transitionEntries)
            {
                _transitionTable.Add(new Tuple<Gamemode, GamemodeTransitionFlag>(data.FindById(entry.modeIn), entry.transition), data.FindById(entry.modeOut));
            }
            
            _transitionTimer = TimerManager.instance.CreateTimer(Game.instance, () => Command(GamemodeTransitionFlag.Timeout));
            EnterMode(data.FindById(data.startingGameModeId));
        }
        
        public void Command(GamemodeTransitionFlag transition)
        {
            if (!_transitionTable.TryGetValue(new Tuple<Gamemode, GamemodeTransitionFlag>(_currentGamemode, transition), out var nextMode))
            {
                return;
            }
            
            _currentGamemode.Exit();
            EnterMode(nextMode);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void EnterMode(Gamemode mode)
        {
            Debug.Log($"[GameTransitionTable] Entering mode '{mode.name}'");
            _currentGamemode = mode;
            mode.Enter();
            if (mode.time > 0.0f)
            {
                _transitionTimer.Start(_currentGamemode.time);
            }
        }
    }
}