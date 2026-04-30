using System;
using Eflatun.SceneReference;
using FoxShooter.Characters.Fox;
using FoxShooter.Game;
using FoxShooter.Game.GamemodeGraph.Runtime;
using UnityEngine;

namespace FoxShooter.Props
{
    public class StageEndTrigger : MonoBehaviour
    {
        [SerializeField] public LevelDefinition nextLevel;
        
        private void OnTriggerEnter(Collider other)
        {
            var fox = other.GetComponent<FoxStats>();
            if (!fox)
            {
                return;
            }

            Game.Game.instance.SaveCurrentCombo();
            Game.Game.instance.nextLevel = nextLevel;
            Game.Game.instance.Command(GamemodeTransitionFlag.Advance);
        }
    }
}