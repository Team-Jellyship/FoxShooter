using System.Collections.Generic;
using FoxShooter.Characters;
using FoxShooter.Characters.AI.Polestar;
using UnityEngine;

namespace Characters.AI.Polestar.Entries
{
    [CreateAssetMenu(fileName = "PsE_CastToGround", menuName = "Polestar/Stack/CastToGround")]
    public class CastToGround : PolestarEntry
    {
        [SerializeField] private float maxLength;
        [SerializeField] private LayerMask mask;
        
        public override void Evaluate(ref List<PolestarResult> results, in PolestarContext context)
        {
            for (var i = results.Count - 1; i >= 0 ; --i)
            {
                if (!Physics.Raycast(results[i].position, Vector3.down, out var hit, maxLength, mask, QueryTriggerInteraction.Ignore))
                {
                    results.RemoveAt(i);
                    continue;
                }

                results[i].position = hit.point;
            }
        }
    }
}