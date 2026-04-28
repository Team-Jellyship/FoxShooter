using System;
using FoxShooter.Game.Combo;
using UnityEngine;
using UnityEngine.Events;

namespace FoxShooter.Characters.Fox
{
    public class ComboMeter : MonoBehaviour
    {
        [field: SerializeField] public float maxCombo { get; private set; } = 100.0f;
        [SerializeField] private float comboDecayRate = 1.0f;
        [SerializeField] private ComboTable combos;

        public UnityEvent<float> comboValueChanged;

        public float currentComboValue { get; private set; }

        private void Update()
        {
            if (currentComboValue == 0.0f)
            {
                return;
            }

            currentComboValue -= comboDecayRate * Time.deltaTime;
            if (currentComboValue < 0.0f)
            {
                currentComboValue = 0.0f;
            }
            comboValueChanged.Invoke(currentComboValue);
        }

        public void SendComboEvent(string comboTag)
        {
            if (!combos.TryGetValue(comboTag, out var combo))
            {
                return;
            }

            currentComboValue = MathF.Min(currentComboValue + combo.comboAmount, maxCombo);
            comboValueChanged.Invoke(currentComboValue);
        }
    }
}