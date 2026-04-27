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

        private bool _doneLooking;
        private KinematicCharacter _character;
        
        protected override Status OnStart()
        {
            _character = agent.Value.GetComponent<KinematicCharacter>();

            if (!_character)
            {
                return Status.Failure;
            }

            _doneLooking = false;
            _character.LookAt(target.Value.position);
            _character.lookAtComplete.AddListener(LookAtDone);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (!_doneLooking)
            {
                return Status.Running;
            }
            _character.lookAtComplete.RemoveListener(LookAtDone);
            return Status.Success;

        }

        private void LookAtDone()
        {
            _doneLooking = true;
            Debug.Log("Done rotating");
        }
    }
}