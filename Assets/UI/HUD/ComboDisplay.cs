using FoxShooter.Characters.Fox;
using UnityEngine;

namespace FoxShooter.UI.HUD
{
    public class ComboDisplay : MonoBehaviour
    {
        [SerializeField] private ComboMeter meter;
        [SerializeField] private ProgressBar bar;

        private void Start()
        {
            meter.comboValueChanged.AddListener(ComboChanged);
            ComboChanged(meter.currentComboValue);
        }

        private void ComboChanged(float comboValue)
        {
            bar.SetPercentage(comboValue / meter.maxCombo);
        }
    }
}