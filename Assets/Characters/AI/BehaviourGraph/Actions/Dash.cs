using FoxShooter.Characters;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace FoxShooter.Characters.AI.BehaviourGraph.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Dash",
        story: "Dash using [DashAbility]",
        category: "Action/FoxShooter",
        id: "ec378fe386feee1ab4c622e647ff317d")]
    
    public class Dash : Action
    {
        [SerializeReference] public BlackboardVariable<DashAbility> dashAbility;

        private bool _awaitingCooldown;
        
        protected override Status OnStart()
        {
            if (dashAbility == null)
            {
                return Status.Failure;
            }
            
            dashAbility.Value.Dash();
            dashAbility.Value.cooldownEnded.AddListener(CooldownEnded);
            _awaitingCooldown = true;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_awaitingCooldown)
            {
                return Status.Running;
            }
            
            dashAbility.Value.cooldownEnded.RemoveListener(CooldownEnded);
            return Status.Success;
        }

        private void CooldownEnded()
        {
            _awaitingCooldown = false;
        }
    }
}