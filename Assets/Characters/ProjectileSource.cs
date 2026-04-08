using System;
using FoxShooter.Game;
using FoxShooter.Scripts;
using Props.Projectiles;
using UnityEditor;
using UnityEngine;

namespace FoxShooter.Characters
{
    public class ProjectileSource : MonoBehaviour
    {
        [SerializeField] private Transform origin;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] [Min(0.0f)] private float cooldownTime;
        [SerializeField] [Min(0.0f)] private float speed;
        [SerializeField] [Min(0.0f)] private float lifetime;

        [SerializeField] private CharacterStats owner;
        
        private bool _onCooldown;
        private TimerHandle _cooldownTimer;

        private void Start()
        {
            _cooldownTimer = TimerManager.instance.CreateTimer(this, () => _onCooldown = false);
        }

        // Input listener. Right now, uses SendMessage, but it should probably use UnityEvents
        private void OnFire()
        {
            Fire();
        }

        public void Fire()
        {
            if (_onCooldown)
            {
                return;
            }
            SpawnProjectile();
            _onCooldown = true;
            _cooldownTimer.Start(cooldownTime);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SpawnProjectile()
        {
            if (!projectilePrefab)
            {
                return;
            }

            var bullet = Instantiate(projectilePrefab, origin.transform.position, origin.transform.rotation);
            bullet.GetComponent<KinematicProjectile>()?.Setup(owner, origin, speed, lifetime);
        }

        private void OnDrawGizmos()
        {
            if (origin)
            {
                StarDebug.DrawArrow(origin.position, origin.position + origin.forward, Color.violetRed);
            }
        }
    }
}