using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using FoxShooter.Scripts;

namespace FoxShooter.Characters.Fox
{
    public class DashAbility : MonoBehaviour
    {
        private readonly static int DashParameter = Animator.StringToHash("Dash");

        private Animator _animator;
        private CharacterStats _stats;
        private StatusEffectInstance _dashStun;
        
        private void Awake()
        {
            _animator = this.GetComponentInRoot<Animator>();
            _stats = this.GetComponentInRoot<CharacterStats>();
        }

        private void Start()
        {
            _dashStun = new StatusEffectInstance(Game.Game.instance.statusEffects.stunned, this);
        }

        private void OnDash()
        {
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