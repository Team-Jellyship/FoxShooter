using FoxShooter.Characters.Fox;
using UnityEngine;

namespace FoxShooter.UI.HUD
{
    public class ComboDisplay : MonoBehaviour
    {
        [SerializeField] private ComboMeter meter;
        [SerializeField] private RectTransform rect;

        private void Start()
        {
            meter.comboValueChanged.AddListener(ComboChanged);
            ComboChanged(meter.currentComboValue);
        }

        private void ComboChanged(float comboValue)
        {
            var scale = rect.localScale;
            scale.x = comboValue / meter.maxCombo;
            rect.localScale = scale;
        }
    }
}