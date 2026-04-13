using System.Collections.Generic;
using FoxShooter.Characters;
using FoxShooter.Characters.AI.Polestar;
using UnityEngine;

namespace Characters.AI.Polestar.Entries
{
    [CreateAssetMenu(fileName = "PsE_InNavMesh", menuName = "Polestar/Stack/InNavMesh")]
    public class InNavMesh : PolestarEntry
    {
        public override void Evaluate(ref List<PolestarResult> results, CharacterStats self, CharacterStats other)
        {
            results.RemoveAll(result => !PolestarNavigation.IsLocationInNavMesh(result.position));
        }
    }
}