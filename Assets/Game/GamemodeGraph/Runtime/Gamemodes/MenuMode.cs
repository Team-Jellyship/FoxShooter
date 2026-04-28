using System;
using UnityEngine;

namespace FoxShooter.Game.GamemodeGraph.Runtime
{
    [Serializable]
    public class MenuMode : Gamemode
    {
        [SerializeReference]
        public GameObject scene;

        [SerializeField] public bool showMouse;

        public override void Enter()
        {
            if (showMouse)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
            if (scene != null)
            {
                Game.instance.LoadMenu(scene);
            }
        }

        public override void Exit()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
            
            Game.instance.UnloadMenu();
        }
    }
}