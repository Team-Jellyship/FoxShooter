using System;
using FoxShooter.Game;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace FoxShooter.Characters.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(KinematicCharacter))]
    public class KinematicAgent : MonoBehaviour
    {
        [SerializeField] private CharacterStats target;
        
        private NavMeshAgent _agent;
        private KinematicCharacter _character;
        private TimerHandle _updateTimer;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _character = GetComponent<KinematicCharacter>();
        }

        private void Start()
        {
            _agent.updatePosition = false;
            // _agent.updateRotation = false;

            _updateTimer = TimerManager.instance.CreateTimer(this, SetDestination);
            _updateTimer.Start(1.0f, true);
        }

        private void FixedUpdate()
        {
            _character.MoveInput(_agent.desiredVelocity.To2D());
            _agent.nextPosition = _character.transform.position;
        }

        private void SetDestination()
        {
            if (target)
            {
                _agent.SetDestination(target.gameObject.transform.position);
            }
        }
    }
}