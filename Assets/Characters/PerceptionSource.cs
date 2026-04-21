using System;
using FoxShooter.Game;
using UnityEngine;

namespace FoxShooter.Characters
{
    public class PerceptionSource : MonoBehaviour
    {
        private void Start()
        {
            PerceptionSubsystem.instance.RegisterPerceptionSource(this);
        }
    }
}