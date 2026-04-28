using System;
using UnityEngine;

namespace Props.Projectiles
{
    public class Missile : KinematicProjectile
    {
        [SerializeField] private float rotationSpeed;

        private void FixedUpdate()
        {
            if (!target)
            {
                return;
            }
            var delta = target.transform.position - transform.position;
            var desiredRotation = Quaternion.LookRotation(delta, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.fixedDeltaTime);
            body.linearVelocity = speed * transform.forward;
        }
    }
}