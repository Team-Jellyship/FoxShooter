using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FoxShooter.Game;
using UnityEngine;
using FoxShooter.Scripts;

namespace FoxShooter.Characters.Fox
{
    public class DashAbility : MonoBehaviour
    {
        private readonly static int DashParameter = Animator.StringToHash("Dash");

        [SerializeField] [Min(0.0f)] private float cooldownTime = 5.0f;
        
        private Animator _animator;
        private CharacterStats _stats;
        private StatusEffectInstance _dashStun;
        private TimerHandle _cooldownTimer;
        private bool _onCooldown;
        
        private void Awake()
        {
            _animator = this.GetComponentInRoot<Animator>();
            _stats = this.GetComponentInRoot<CharacterStats>();
        }

        private void Start()
        {
            _dashStun = new StatusEffectInstance(Game.Game.instance.statusEffects.stunned, this);
            _cooldownTimer = TimerManager.instance.CreateTimer(this, () => _onCooldown = false);
        }

        private void OnDash()
        {
            if (_onCooldown)
            {
                return;
            }
            
            _cooldownTimer.Start(cooldownTime);
            _onCooldown = true;
            _animator.SetTrigger(DashParameter);
        }

        public void ApplyStun()
        {
            _stats.ApplyStatusEffect(_dashStun);
        }

        public void RemoveStun()
        {
            _stats.RemoveStatusEffectInstance(_dashStun);
        }
    }
}