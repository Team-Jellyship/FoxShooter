using UnityEngine;

namespace FoxShooter.Game.StatusEffects
{
    [CreateAssetMenu(menuName = "Status Effects/Status Effect List")]
    public class StatusEffectList : ScriptableObject
    {
        [field: SerializeField] public StatusEffect health { get; private set; }
        [field: SerializeField] public StatusEffect maxHealth { get; private set; }
        [field: SerializeField] public StatusEffect invulnerability { get; private set; }
    }
}