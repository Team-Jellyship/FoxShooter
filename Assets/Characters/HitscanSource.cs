using System;
using FoxShooter.Game;
using FoxShooter.Props.Projectiles;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FoxShooter.Characters
{
    public class HitscanSource : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float cooldownTime;
        [SerializeField] [Min(0.0f)] private float distance;
        [SerializeField] private float damage;
        [SerializeField] private ViewRecoil viewRecoil;
        [SerializeField] private LayerMask mask;
        [SerializeField] private UnityEvent fired;
        [SerializeField] private UnityEvent hitTarget;
        [SerializeField] private CharacterStats owner;

        [Header("Effect")]
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private float hitSpeed;
        [SerializeField] private Vector3 effectOffset;
        [SerializeField] private float effectStartTime;
        
        private bool _onCooldown;
        private TimerHandle _cooldownTimer;
        
        private void Start()
        {
            _cooldownTimer = TimerManager.instance.CreateTimer(this, () => _onCooldown = false);
        }

        public void Fire(InputAction.CallbackContext context)
        {
            if (Game.Game.instance.paused)
            {
                return;
            }
            if (context.phase == InputActionPhase.Performed)
            {
                Fire();
            }
        }

        private void Fire()
        {
            if (!CanFire())
            {
                return;
            }
            
            fired.Invoke();
            var hitTrail = Instantiate(hitEffect);
            var gunPosition = transform.position +
                transform.right * effectOffset.x +
                transform.up * effectOffset.y +
                transform.forward * effectOffset.z;
            
            if (Physics.Raycast(transform.position, transform.forward,
                    out var result, distance, mask, QueryTriggerInteraction.Collide))
            {
                var stats = result.transform.gameObject.GetComponentInRoot<CharacterStats>();
                if (stats && stats != owner)
                {
                    hitTarget.Invoke();
                    stats.TakeDamage(damage, owner, false);
                }
                
                hitTrail.GetComponent<HitscanParticle>()?.SetPath(gunPosition, result.point, hitSpeed, effectStartTime);
            }
            else
            {
                hitTrail.GetComponent<HitscanParticle>()?.SetPath(gunPosition, transform.position + transform.forward * distance, hitSpeed, effectStartTime);
            }
            
            
            if (viewRecoil != null)
            {
                viewRecoil.AddRecoil();
            }

            _onCooldown = true;
            _cooldownTimer.Start(cooldownTime);
            Fired();
        }

        protected virtual bool CanFire()
        {
            return !_onCooldown;
        }

        protected virtual void Fired()
        {
            
        }
    }
}