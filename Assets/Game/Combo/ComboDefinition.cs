using System;

namespace FoxShooter.Game.Combo
{
    [Serializable]
    public struct ComboDefinition
    {
        public string tag;
        public string description;
        public float comboAmount;
    }
}