using System.Collections.Generic;
using Characters.AI.Polestar;
using FoxShooter.Scripts;
using UnityEngine;

namespace FoxShooter.Characters.AI.Polestar
{
    [CreateAssetMenu(fileName = "PsE_PointOnCircle", menuName = "Polestar/Stack/PointOnCircle")]
    public class PointOnCircle : PolestarEntry
    {
        [SerializeField] [Min(0.0f)] private float radius;
        [SerializeField] [Min(1)] private int numPoints = 10;
        
        public override void Evaluate(ref List<PolestarResult> results, in PolestarContext context)
        {
            var newList = new List<PolestarResult>();
            
            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var result in results)
            {
                newList.AddRange(GetPointsOnRadius(result.position, radius, numPoints, result.score));
            }
            results = newList;
        }
        
        private static List<PolestarResult> GetPointsOnRadius(Vector3 center, float circleRadius, int num, float score)
        {
            var result = new List<PolestarResult>(num);
            var interval = Mathf.PI * 2.0f / num;
            for (var i = 0; i < num; ++i)
            {
                var newPosition = center;
                newPosition.x += Mathf.Cos(interval * i) * circleRadius;
                newPosition.z += Mathf.Sin(interval * i) * circleRadius;
                result.Add(new PolestarResult(newPosition, score));
            }

            return result;
        }
    }
}