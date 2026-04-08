using System;
using FoxShooter.Game;
using Props.Projectiles;
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

        private void OnFire()
        {
            if (_onCooldown)
            {
                return;
            }
            SpawnProjectile();
            _onCooldown = true;
            _cooldownTimer.Start(cooldownTime);
        }

        private void SpawnProjectile()
        {
            if (!projectilePrefab)
            {
                return;
            }

            var bullet = Instantiate(projectilePrefab, origin.transform.position, origin.transform.rotation);
            bullet.GetComponent<KinematicProjectile>()?.Setup(owner, origin, speed, lifetime);
        }
    }
}