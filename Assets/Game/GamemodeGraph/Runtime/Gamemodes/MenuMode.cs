using System;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public class MenuMode : Gamemode
    {
        [SerializeReference]
        public GameObject scene;
    }
}