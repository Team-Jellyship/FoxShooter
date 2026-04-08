using System;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public enum GamemodeTransitionFlag
    {
        Timeout,
        Advance,
        Loaded
    }
    
    [Serializable]
    public class Gamemode
    {
        [SerializeField]
        public string id;
        
        [SerializeField]
        public string name;

        [SerializeField]
        public float time;
        
        public virtual void Enter()
        { }
        
        public virtual void Exit()
        { }
    }
}