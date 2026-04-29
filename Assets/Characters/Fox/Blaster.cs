using System;
using UnityEngine;
using UnityEngine.Events;

namespace FoxShooter.Characters.Fox
{
    public class Blaster : HitscanSource
    {
        [SerializeField] private float currentHeat = 0.0f;
        [SerializeField] private float maxHeat = 100.0f;
        [SerializeField] private float heatPerShot = 10.0f;
        [SerializeField] private float passiveCooldownRate = 5.0f;

        [SerializeField] private UnityEvent overheat;
        
        // How long it takes for the gun to cool down after it overheats
        [SerializeField] private float activeCooldownRate = 1.0f;

        private bool _overheating;

        protected override bool CanFire()
        {
            return base.CanFire() && !_overheating;
        }

        protected override void Fired()
        {
            currentHeat += heatPerShot;
            if (currentHeat < maxHeat)
            {
                return;
            }
            
            
            currentHeat = maxHeat;
            _overheating = true;
            overheat.Invoke();
        }

        private void Update()
        {
            if (currentHeat == 0.0f)
            {
                return;
            }

            currentHeat -= (_overheating ? activeCooldownRate : passiveCooldownRate) * Time.deltaTime;
            if (!(currentHeat <= 0.0f))
            {
                return;
            }
            _overheating = false;
            currentHeat = 0.0f;
        }
    }
}