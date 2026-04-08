using System;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public class MenuMode : Gamemode
    {
        [SerializeReference]
        public GameObject scene;

        public override void Enter()
        {
            if (scene != null)
            {
                Game.instance.LoadMenu(scene);
            }
        }

        public override void Exit()
        {
            Game.instance.UnloadMenu();
        }
    }
}