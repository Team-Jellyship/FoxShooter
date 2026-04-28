using FoxShooter.Game;
using UnityEngine;
using FoxShooter.Scripts;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FoxShooter.Characters
{
    public class DashAbility : MonoBehaviour
    {
        private readonly static int DashParameter = Animator.StringToHash("Dash");

        [SerializeField] [Min(0.0f)] private float cooldownTime = 5.0f;
        [SerializeField] private float healAmount = 1.0f;
        [SerializeField] private UnityEvent hitEnemy;

        public UnityEvent cooldownEnded;
        
        private bool _onCooldown;
        private TimerHandle _cooldownTimer;
        private StatusEffectInstance _dashStun;
        private StatusEffectInstance _dashInvuln;
        
        private Animator _animator;
        private CharacterStats _stats;
        private ContactHitbox _hitbox;
        
        private void Awake()
        {
            // Grab cached references for every component we need later
            // I'm assuming that these will be on the Prefab's root,
            // but that isn't always necessarily the case
            _animator = this.GetComponentInRoot<Animator>();
            _stats = this.GetComponentInRoot<CharacterStats>();
        }

        private void Start()
        {
            // Probably need to track whether the enemies were killed by this attack or
            // a latent bullet
            
            // Listen for when the owner kills something, and reset the cooldown if that happens
            _stats.onKillCharacter.AddListener(KilledEnemy);
            
            // Create effect instances. We need to track this to easily remove, otherwise these would
            // have to be set by duration
            _dashStun = new StatusEffectInstance(Game.Game.instance.statusEffects.stunned, this);
            _dashInvuln = new StatusEffectInstance(Game.Game.instance.statusEffects.invulnerability, this);
            
            // Create our cooldown timer, but don't start it yet
            _cooldownTimer = TimerManager.instance.CreateTimer(this, EndCooldown);
        }
        
        public void Dash(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }
            
            Dash();
        }

        public void Dash()
        {
            if (_onCooldown)
            {
                return;
            }
            
            _cooldownTimer.Start(cooldownTime);
            _onCooldown = true;
            _animator.SetTrigger(DashParameter);
        }

        // Animation event hooks!
        // The animator can access private methods,so these
        // are placed on the timeline and called based on the
        // current animation, rather than being defined
        // entirely programmatically
        private void ApplyStun()
        {
            _stats.ApplyStatusEffect(_dashStun);
        }

        private void RemoveStun()
        {
            _stats.RemoveStatusEffectInstance(_dashStun);
        }

        private void ApplyInvuln()
        {
            _stats.ApplyStatusEffect(_dashInvuln);
        }

        private void RemoveInvuln()
        {
            _stats.RemoveStatusEffectInstance(_dashInvuln);
        }
        // End of animation event hooks

        private void KilledEnemy(CharacterStats enemy, DamageType type)
        {
            // Technically any damage type can restore the cooldown
            if (type != DamageType.Melee)
            {
                return;
            }
            hitEnemy.Invoke();
            _stats.Heal(healAmount);
            EndCooldown();
        }

        private void EndCooldown()
        {
            _cooldownTimer.Pause();
            _onCooldown = false;
            cooldownEnded.Invoke();
        }
    }
}