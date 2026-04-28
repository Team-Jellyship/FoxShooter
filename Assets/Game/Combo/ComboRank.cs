using System;

namespace FoxShooter.Game.Combo
{
    [Serializable]
    public struct ComboRank
    {
        public string name;
        public float decayRate;
        public float pointsToNextRank;
        public float startingPoints;
    }
}