using System.Collections.Generic;
using Characters.AI.Polestar;
using UnityEngine;

namespace FoxShooter.Characters.AI.Polestar
{
    [CreateAssetMenu(fileName = "PsS_Stack", menuName = "Polestar/Stack/Stack", order = 0)]
    public class PolestarStack : ScriptableObject
    {
        [SerializeField] private List<PolestarEntry> queries = new();

        public List<PolestarResult> Evaluate(in PolestarContext context)
        {
            var result = new List<PolestarResult> { new (context.target ? context.target.transform.position : Vector3.zero, 1.0f) };
            foreach (var entry in queries)
            {
                entry.Evaluate(ref result, context);
                if (result.Count == 0)
                {
                    return result;
                }
            }

            PolestarResult.NormalizeScore(ref result);
            return result;
        }
    }
}