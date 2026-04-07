using System;
using FoxShooter.Scripts;
using UnityEngine;

namespace FoxShooter.Characters
{
    [RequireComponent(typeof(Collider))]
    public class ContactHitbox : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.GetComponentInRoot<CharacterStats>()?.TakeDamage(1.0f, null, false);
        }
    }
}