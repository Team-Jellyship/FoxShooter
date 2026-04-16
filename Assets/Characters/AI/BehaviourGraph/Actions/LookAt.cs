using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace FoxShooter.Characters.AI.BehaviourGraph.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "LookAtAction",
        story: "[Agent] looks at [Target]",
        category: "Action/FoxShooter",
        id: "40419517b30d00fb07be5845ccf8974e")]
    public class LookAt : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> agent;
        [SerializeReference] public BlackboardVariable<Transform> target;

        protected override Status OnStart()
        {
            var kinematicCharacterController = agent.Value.GetComponent<KinematicCharacter>();

            if (!kinematicCharacterController)
            {
                return Status.Failure;
            }
    
            kinematicCharacterController.LookAt(target.Value.position);
            return Status.Success;
        }
    }
}