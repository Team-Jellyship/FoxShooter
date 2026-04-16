using System;
using Characters.AI.Polestar;
using FoxShooter.Characters.AI.Polestar;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace FoxShooter.Characters.AI.BehaviourGraph.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "PolestarQuery",
        story: "Run a polestar stack [Query] and write it to [position]",
        category: "Action/Polestar",
        id: "b42103bd915d5fd1b2d574b7f2d45bb0")]
    public class PolestarQuery : Action
    {

        [SerializeReference] public BlackboardVariable<Transform> self;
        [SerializeReference] public BlackboardVariable<Transform> target;
        [SerializeReference] public BlackboardVariable<PolestarStack> query;
        [SerializeReference] public BlackboardVariable<Transform> position;


        protected override Status OnStart()
        {
            var context = new PolestarContext()
            {
                self = self.Value,
                target = target.Value
            };
            var result = query.Value.Evaluate(context);
            if (result.Count == 0)
            {
                return Status.Failure;
            }

            position.Value.position = result[PolestarResult.Max(ref result)].position;
            return Status.Success;
        }
    }
}