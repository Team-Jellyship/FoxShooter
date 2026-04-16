using System.Collections.Generic;
using Characters.AI.Polestar;
using FoxShooter.Scripts;
using UnityEngine;

namespace FoxShooter.Characters.AI.Polestar
{
    [CreateAssetMenu(fileName = "PsE_PointInRadius", menuName = "Polestar/Stack/PointInRadius")]
    public class PointInRadius : PolestarEntry
    {
        [SerializeField] [Min(0.0f)] private float minRadius;
        [SerializeField] [Min(0.0f)] private float maxRadius;
        [SerializeField] [Min(1)] private int maxIterations = 10;
        
        public override void Evaluate(ref List<PolestarResult> results, in PolestarContext context)
        {
            var newList = new List<PolestarResult>();
            
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var result in results)
            {
                newList.Add(new PolestarResult(RandomPoint(result.position), result.score));
            }
            results = newList;
        }

        private Vector3 RandomPoint(Vector3 center)
        {
            for (var i = 0; i < maxIterations; ++i)
            {
                var result = StarMath.GetRandomPointInRadius(center, maxRadius, minRadius);
                if (PolestarNavigation.IsLocationInNavMesh(result))
                {
                    return result;
                }
            }

            return center;
        }
    }
}