using System;
using FoxShooter.Game;
using FoxShooter.Scripts;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace FoxShooter.Characters.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(KinematicCharacter))]
    public class KinematicAgent : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private KinematicCharacter _character;
        private TimerHandle _updateTimer;
        private GameObject _locationProxy;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _character = GetComponent<KinematicCharacter>();
        }

        private void Start()
        {
            _agent.updatePosition = false;
            _agent.updateRotation = false;

            var behaviorGraph = GetComponent<BehaviorGraphAgent>();
            if (!behaviorGraph)
            {
                return;
            }
            
            _locationProxy = new GameObject($"{name}_LocationProxy")
            {
                transform =
                {
                    position = transform.position
                }
            };

            behaviorGraph.SetVariableValue("LocationProxy", _locationProxy.transform);
        }

        private void FixedUpdate()
        {
            _character.MoveInput(_agent.desiredVelocity.To2D());
            _agent.nextPosition = _character.transform.position;
        }
    }
}