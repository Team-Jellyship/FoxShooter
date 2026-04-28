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
        
        protected CharacterStats target;
        protected Rigidbody body;
        private ContactHitbox _hitbox;
        private TimerHandle _lifetime;
        
        private void Awake()
        {
            _hitbox = GetComponent<ContactHitbox>();
            body = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            _hitbox.hit.AddListener(() => hit.Invoke());
        }

        public void Setup(CharacterStats owner, CharacterStats targetIn, Transform origin, float speedIn, float lifetime)
        {
            _lifetime ??= TimerManager.instance.CreateTimer(this, Expire);
            speed = speedIn;
            body.linearVelocity = origin.transform.forward * speed;
            _hitbox.owner = owner;
            _lifetime.Start(lifetime);
            target = targetIn;
        }

        private void Expire()
        {
            Destroy(gameObject);
        }
    }
}