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
        [SerializeField] private LayerMask mask;

        [SerializeField] private UnityEvent fired;

        private bool _onCooldown;
        private TimerHandle _cooldownTimer;
        private CharacterStats _owner;
        
        private void Start()
        {
            _cooldownTimer = TimerManager.instance.CreateTimer(this, () => _onCooldown = false);
        }

        public void Fire(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Fire();
            }
        }

        private void Fire()
        {
            if (_onCooldown)
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
                    stats.TakeDamage(damage, _owner, false);
                }
            }

            _onCooldown = true;
            _cooldownTimer.Start(cooldownTime);
        }
    }
}