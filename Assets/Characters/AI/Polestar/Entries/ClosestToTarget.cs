using System.Collections.Generic;
using Characters.AI.Polestar;
using UnityEngine;

namespace FoxShooter.Characters.AI.Polestar.Entries
{
    [CreateAssetMenu(fileName = "PsE_ClosestToTarget", menuName = "Polestar/Stack/ClosestToTarget")]
    public class ClosestToTarget : PolestarEntry
    {
        public override void Evaluate(ref List<PolestarResult> results, CharacterStats self, CharacterStats other)
        {
            PolestarResult.Score(ref results, result => -Vector3.Distance(result.position, other.transform.position));
        }
    }
}