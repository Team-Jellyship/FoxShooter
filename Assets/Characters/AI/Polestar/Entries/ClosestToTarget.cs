using System.Collections.Generic;
using Characters.AI.Polestar;
using UnityEngine;

namespace FoxShooter.Characters.AI.Polestar.Entries
{
    [CreateAssetMenu(fileName = "PsE_ClosestToTarget", menuName = "Polestar/Stack/ClosestToTarget")]
    public class ClosestToTarget : PolestarEntry
    {
        public override void Evaluate(ref List<PolestarResult> results, in PolestarContext context)
        {
            var position = context.target.position;
            PolestarResult.Score(ref results, result => -Vector3.Distance(result.position, position));
        }
    }
}