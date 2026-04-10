using System.Collections.Generic;
using Characters.AI.Polestar;
using UnityEngine;

namespace FoxShooter.Characters.AI.Polestar.Entries
{
    [CreateAssetMenu(fileName = "PsE_ClosestToSelf", menuName = "Polestar/Stack/ClosestToSelf")]
    public class ClosestToSelf : PolestarEntry
    {
        public override void Evaluate(ref List<PolestarResult> results, CharacterStats self, CharacterStats other)
        {
            PolestarResult.Score(ref results, result => -Vector3.Distance(result.position, self.transform.position));
        }
    }
}