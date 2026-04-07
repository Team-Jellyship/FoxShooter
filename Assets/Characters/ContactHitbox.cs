using System;
using FoxShooter.Scripts;
using UnityEngine;

namespace FoxShooter.Characters
{
    [RequireComponent(typeof(Collider))]
    public class ContactHitbox : MonoBehaviour
    {
        [SerializeField] [Min(0.0f)] private float damage = 1.0f;
        
        private CharacterStats _stats;
        
        private void Awake()
        {
            _stats = this.GetComponentInRoot<CharacterStats>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponentInRoot<CharacterStats>()?.TakeDamage(damage, _stats, false);
        }
    }
}