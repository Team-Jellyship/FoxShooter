using FoxShooter.Game;
using FoxShooter.Game.GamemodeGraph.Runtime;
using UnityEngine;

namespace UI.Menus
{
    public class CommandButton : MonoBehaviour
    {
        [SerializeField] private GamemodeTransitionFlag command;
        
        public void Execute()
        {
            Game.instance.Command(command);
        }
    }
}