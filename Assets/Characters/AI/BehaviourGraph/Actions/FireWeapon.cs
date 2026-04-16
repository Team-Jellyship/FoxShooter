using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace FoxShooter.Characters.AI.BehaviourGraph.Actions
{
    [Serializable] [GeneratePropertyBag]
    [NodeDescription(
        name: "FireProjectile",
        story: "[Self] fires [Projectile]",
        category: "Action",
        id: "81813bc11c702048994bfc4c6a8920e6")]
    
    public class FireWeapon : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> agent;
        [SerializeReference] public BlackboardVariable<bool> waitForAttackCompletion;

        [CreateProperty] private bool _completed;
        
        protected override Status OnStart()
        {
            var attackController = agent.Value.GetComponent<ProjectileSource>();
            if (attackController == null)
            {
                return Status.Failure;
            }
        
            attackController.Fire();
            if (!waitForAttackCompletion.Value)
            {
                return Status.Success;
            }
        
            attackController.onCooldownEnded.AddListener(CooldownEnded);
            _completed = false;
            return Status.Running;

        }

        protected override Status OnUpdate()
        {
            return _completed ? Status.Success : Status.Running;
        }

        private void CooldownEnded()
        {
            if (!waitForAttackCompletion.Value)
            {
                return;
            }
            var attackController = agent.Value.GetComponent<ProjectileSource>();
            attackController.onCooldownEnded.RemoveListener(CooldownEnded);
            _completed = true;
        }
    }
}