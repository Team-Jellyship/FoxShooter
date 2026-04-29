using System;

namespace FoxShooter.Game
{
    [Serializable]
    public struct LevelRank
    {
        public static LevelRank unranked
        {
            get => new LevelRank
            {
                name = "Unranked",
                score = 0.0f
            };
        }


        public string name;
        public float score;
    }
}