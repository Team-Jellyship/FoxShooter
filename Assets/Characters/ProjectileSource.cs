using System;
using FoxShooter.Game;
using FoxShooter.Scripts;
using Props.Projectiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FoxShooter.Characters
{
    public class ProjectileSource : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] [Min(0.0f)] private float cooldownTime;
        [SerializeField] [Min(0.0f)] private float speed;
        [SerializeField] [Min(0.0f)] private float lifetime;
        
        [SerializeField] private Animator _animator; // For Animation Triggers
        [SerializeField] private CharacterStats owner;

        public UnityEvent onCooldownEnded;
        
        private bool _onCooldown;
        private TimerHandle _cooldownTimer;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _cooldownTimer = TimerManager.instance.CreateTimer(this, () =>
            {
                _onCooldown = false;
                onCooldownEnded.Invoke();
            });
        }
        
        public void Fire(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }
            Fire();
        }

        public void Fire()
        {
            _animator.SetTrigger("Fire");
            
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

            var bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
            bullet.GetComponent<KinematicProjectile>()?.Setup(owner, transform, speed, lifetime);
        }

        private void OnDrawGizmos()
        {
            StarDebug.DrawArrow(transform.position, transform.position + transform.forward, Color.violetRed);
        }
    }
}