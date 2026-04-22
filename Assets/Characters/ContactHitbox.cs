using System;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FoxShooter.Characters
{
    [RequireComponent(typeof(Collider))]
    public class ContactHitbox : MonoBehaviour
    {
        [SerializeField] private DamageType damageType = DamageType.Unaspected;
        [SerializeField] [Min(0.0f)] private float damage = 1.0f;

        public UnityEvent hit;
        
        public CharacterStats owner;
        
        private void Awake()
        {
            owner = this.GetComponentInRoot<CharacterStats>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ignoreDamage"))
            {
                return;
            }
            
            var otherStats = other.GetComponentInRoot<CharacterStats>();
            if (otherStats == null || otherStats == owner)
            {
                return;
            }
            hit.Invoke();
            otherStats.TakeDamage(damage, owner, false, damageType);
        }
    }
}