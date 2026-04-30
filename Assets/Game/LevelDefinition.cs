using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace FoxShooter.Game
{
    [CreateAssetMenu(fileName = "L_Level", menuName = "FoxShooter/Level")]
    public class LevelDefinition : ScriptableObject
    {
        [SerializeField] public SceneReference scene;
        [SerializeField] public Sprite thumbnail;
        [SerializeField] public string displayName;
        [SerializeField] public List<LevelRank> ranks;
        [SerializeField] public float highScore;

        private void OnValidate()
        {
            ranks.Sort((a, b) => -a.score.CompareTo(b.score));
        }

        public LevelRank GetRank(float score)
        {
            if (ranks.Count == 0)
            {
                return LevelRank.unranked;
            }
            
            for (var i = ranks.Count - 1; i >= 0; --i)
            {
                if (score < ranks[i].score)
                {
                    return i == ranks.Count - 1 ? LevelRank.unranked : ranks[i + 1];
                }
            }

            return ranks[0];
        }

        public LevelRank GetBestRank()
        {
            return GetRank(highScore);
        }
    }
}