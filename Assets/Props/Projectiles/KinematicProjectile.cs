using System;
using FoxShooter.Characters;
using FoxShooter.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Props.Projectiles
{
    [RequireComponent(typeof(ContactHitbox))]
    [RequireComponent(typeof(Rigidbody))]
    public class KinematicProjectile : MonoBehaviour
    {
        public UnityEvent hit;
        public float speed { get; private set; }
        
        private ContactHitbox _hitbox;
        private Rigidbody _body;
        private TimerHandle _lifetime;
        
        private void Awake()
        {
            _hitbox = GetComponent<ContactHitbox>();
            _body = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _hitbox.hit.AddListener(() => hit.Invoke());
        }

        public void Setup(CharacterStats owner, Transform origin, float speedIn, float lifetime)
        {
            _lifetime ??= TimerManager.instance.CreateTimer(this, Expire);
            speed = speedIn;
            _body.linearVelocity = origin.transform.forward * speed;
            _hitbox.owner = owner;
            _lifetime.Start(lifetime);
        }

        private void Expire()
        {
            Destroy(gameObject);
        }
    }
}