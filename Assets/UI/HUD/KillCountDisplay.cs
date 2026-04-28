using System;
using FoxShooter.Characters.Fox;
using TMPro;
using UnityEngine;

namespace FoxShooter.UI.HUD
{
    public class KillCountDisplay : MonoBehaviour
    {
        [SerializeField] private ComboMeter comboMeter;
        [SerializeField] private ProgressBar bar;
        [SerializeField] private TextMeshProUGUI textDisplay;

        private void Awake()
        {
            comboMeter.killCountChanged.AddListener(SetKillCount);
            textDisplay.SetText("0");
            bar.SetPercentage(0.0f);
        }

        private void Update()
        {
            bar.SetPercentage(comboMeter.GetKillTimerPercentage());
        }

        private void SetKillCount(int count)
        {
            textDisplay.SetText($"{count}");
        }
    }
}