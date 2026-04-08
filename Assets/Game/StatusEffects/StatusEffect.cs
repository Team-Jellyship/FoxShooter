using System;
using UnityEngine;

namespace FoxShooter.Game.StatusEffects
{
    [Serializable]
    [CreateAssetMenu(menuName = "Status Effects/Status Effect")]
    public class StatusEffect : ScriptableObject
    {
        public string effectName;
    
        public EffectAccumulator accumulator;
    }
}