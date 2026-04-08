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
        private ContactHitbox _hitbox;
        private Rigidbody _body;
        
        private void Awake()
        {
            _hitbox = GetComponent<ContactHitbox>();
            _body = GetComponent<Rigidbody>();
        }


        public void Setup(CharacterStats owner, Transform origin, float speed, float lifetime)
        {
            _body.linearVelocity = origin.transform.forward * speed;
            _hitbox.owner = owner;
            
            TimerManager.instance.CreateTimer(this, Expire).Start(lifetime);
        }

        private void Expire()
        {
            Destroy(gameObject);
        }
    }
}