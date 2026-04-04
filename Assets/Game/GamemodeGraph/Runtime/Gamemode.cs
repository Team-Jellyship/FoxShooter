using System;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public enum GamemodeTransitionFlag
    {
        Timeout,
        Advance
    }
    
    [Serializable]
    public class Gamemode
    {
        public Gamemode(string name, float time)
        {
            this.name = name;
            this.time = time;
        }
        
        [field: SerializeField]
        public string name { get; private set; }

        [field: SerializeField]
        public float time { get; private set; }
        
        public virtual void Enter()
        { }
        
        public virtual void Exit()
        { }
    }
}