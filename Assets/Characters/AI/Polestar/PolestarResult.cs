using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.AI.Polestar
{
    public class PolestarResult
    {
        public Vector3 position;
        public float score;

        public PolestarResult(Vector3 position, float score)
        {
            this.position = position;
            this.score = score;
        }

        public static void Score(ref List<PolestarResult> results, Func<PolestarResult, float> scoreFunction)
        {
            foreach (var result in results)
            {
                result.score = scoreFunction(result);
            }
        }

        public static void NormalizeScore(ref List<PolestarResult> results)
        {
            var max = float.NegativeInfinity;
            var min = float.PositiveInfinity;

            foreach (var result in results)
            {
                max = MathF.Max(result.score, max);
                min = MathF.Min(result.score, min);
            }
            var delta = max - min;
            var rangeFactor = delta == 0.0f ? 1.0f : 1.0f / delta;
            foreach (var result in results)
            {
                result.score = (result.score - min) * rangeFactor;
            }
        }

        public static int Max(ref List<PolestarResult> results)
        {
            var max = 0;
            var maxScore = float.NegativeInfinity;
            
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            for (var i = 0; i < results.Count; ++i)
            {
                if (!(results[i].score > maxScore))
                {
                    continue;
                }
                max = i;
                maxScore = results[i].score;
            }

            return max;
        }

        public static int Min(ref List<PolestarResult> results)
        {
            var min = 0;
            var minScore = float.PositiveInfinity;

            
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            for (var i = 0; i < results.Count; ++i)
            {
                if (!(results[i].score < minScore))
                {
                    continue;
                }
                min = i;
                minScore = results[i].score;
            }

            return min;
        }
    }
}