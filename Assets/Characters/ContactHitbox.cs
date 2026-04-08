using System;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace FoxShooter.Characters
{
    [RequireComponent(typeof(Collider))]
    public class ContactHitbox : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float damage = 1.0f;
        
        public CharacterStats owner;
        
        private void Awake()
        {
            owner = this.GetComponentInRoot<CharacterStats>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var otherStats = other.GetComponentInRoot<CharacterStats>();
            if (otherStats == null || otherStats == owner)
            {
                return;
            }
            otherStats.TakeDamage(damage, owner, false);
        }
    }
}