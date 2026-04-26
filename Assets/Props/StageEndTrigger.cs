using System;
using Eflatun.SceneReference;
using FoxShooter.Characters.Fox;
using FoxShooter.Game.GamemodeGraph.Runtime;
using UnityEngine;

namespace FoxShooter.Props
{
    public class StageEndTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var fox = other.GetComponent<FoxStats>();
            if (!fox)
            {
                return;
            }
            
            Game.Game.instance.Command(GamemodeTransitionFlag.Advance);
        }
    }
}