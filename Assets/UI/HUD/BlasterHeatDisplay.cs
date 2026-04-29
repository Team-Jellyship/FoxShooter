using System;
using FoxShooter.Characters.Fox;
using UnityEngine;

namespace FoxShooter.UI.HUD
{
    public class BlasterHeatDisplay : MonoBehaviour
    {
        [SerializeField] private Blaster blaster;
        [SerializeField] private ProgressBar bar;

        private void Start()
        {
            blaster.heatValueChanged.AddListener(HeatChanged);
            bar.SetPercentage(0.0f);
        }

        private void HeatChanged(float heat)
        {
            bar.SetPercentage(heat);
        }
    }
}