using System;
using FoxShooter.Game;
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
        [SerializeField] public Animator _animator; // For Animation Triggers
        [SerializeField] private LayerMask mask;

        [SerializeField] private UnityEvent fired;
        [SerializeField] private UnityEvent hitTarget;

        private bool _onCooldown;
        private TimerHandle _cooldownTimer;
        private CharacterStats _owner;
        
        private void Start()
        {
            _animator = GetComponentInParent<Animator>();
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
            
            // Debug.DrawRay(transform.position, transform.forward * distance, Color.violetRed, 0.5f);
            if (Physics.Raycast(transform.position, transform.forward,
                    out var result, distance, mask, QueryTriggerInteraction.Collide))
            {
                var stats = result.transform.gameObject.GetComponentInRoot<CharacterStats>();
                if (stats && stats != _owner)
                {
                    hitTarget.Invoke();
                    _animator.SetTrigger("hitMarker");
                    stats.TakeDamage(damage, _owner, false);
                }
                
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