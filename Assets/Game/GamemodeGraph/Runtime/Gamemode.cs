using System;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public enum GamemodeTransitionFlag
    {
        Timeout,
        Advance
    }
    
    public class Gamemode
    {
        public Gamemode(string name)
        {
            this.name = name;
        }
        
        public string name { get; private set; }
    }
}