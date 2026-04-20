using System;
using FoxShooter.Characters;
using TMPro;
using UnityEngine;

namespace FoxShooter.UI.HUD
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private CharacterStats character;

        private TextMeshProUGUI _textMesh;
        
        private void Awake()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            character.RegisterEffectChangedDelegate(Game.Game.instance.statusEffects.health, (_, health) => HealthChanged(health), this);
            character.RegisterEffectChangedDelegate(Game.Game.instance.statusEffects.maxHealth, (_, maxHealth) => MaxHealthChanged(maxHealth), this);
            HealthChanged(character.GetEffectValue(Game.Game.instance.statusEffects.health));
        }
        
        private void HealthChanged(float health)
        {
            _textMesh.text = $"HP: {health:0} / {character.GetEffectValue(Game.Game.instance.statusEffects.maxHealth):0}";
        }

        private void MaxHealthChanged(float maxHealth)
        {
            _textMesh.text = $"HP: {character.GetEffectValue(Game.Game.instance.statusEffects.health):0} / {maxHealth:0}";
        }
    }
}