using System;
using FoxShooter.Game;
using UnityEngine;

namespace FoxShooter.Characters.AI
{
    public class PeriodicShooter : MonoBehaviour
    {
        private ProjectileSource _projectileSource;
        [SerializeField] [Min(0.0f)] private float repeatTime = 1.0f;
        
        private TimerHandle _repeatTimer;
        
        private void Awake()
        {
            _projectileSource = GetComponent<ProjectileSource>();
        }

        private void Start()
        {
            _repeatTimer = TimerManager.instance.CreateTimer(this, Fire);
            _repeatTimer.Start(repeatTime, true);
        }

        private void Fire()
        {
            _projectileSource.Fire();
        }
    }
}