using System;
using FoxShooter.Game;
using FoxShooter.Scripts;
using Props.Projectiles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FoxShooter.Characters.Fox
{
    [RequireComponent(typeof(Collider))]
    public class AirBlastAbility : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float forceProjectileStrength = 1.5f;
        [SerializeField] [Min(0.0f)] private float projectileNewLifespan = 3.0f;
        [SerializeField] [Min(0.0f)] private float cooldownTime = 5.0f;
        [SerializeField] [Min(0.0f)] private float forceWindow = 0.5f;
        [SerializeField] private UnityEvent fired;
        [SerializeField] private UnityEvent reflectBlastHit;
        [SerializeField] private UnityEvent onReflect;
        
        private CharacterStats _stats;
        private Collider _collider;
        private bool _onCooldown;
        private TimerHandle _forceWindowTimer;
        private TimerHandle _cooldownTimer;

        private void Awake()
        {
            _stats = this.GetComponentInRoot<CharacterStats>();
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }

        private void Start()
        {
            _forceWindowTimer = TimerManager.instance.CreateTimer(this, () => _collider.enabled = false);
            _cooldownTimer = TimerManager.instance.CreateTimer(this, () => _onCooldown = false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("projectile"))
            {
                return;
            }

            var projectile = other.GetComponent<KinematicProjectile>();
            if (!projectile)
            {
                return;
            }
            
            projectile.Setup(_stats, transform, projectile.speed * forceProjectileStrength, projectileNewLifespan);
            projectile.transform.rotation = transform.rotation;
            onReflect.Invoke();
            projectile.hit.RemoveListener(ProjectileHit);
            projectile.hit.AddListener(ProjectileHit);
        }

        public void Fire(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }

            if (_onCooldown)
            {
                return;
            }
            
            fired.Invoke();
            _collider.enabled = true;
            _onCooldown = true;
            _forceWindowTimer.Start(forceWindow);
            _cooldownTimer.Start(cooldownTime);
        }

        private void ProjectileHit()
        {
            reflectBlastHit.Invoke();
        }
    }
}