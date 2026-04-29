using System;
using TMPro;
using UnityEngine;

namespace FoxShooter.UI.HUD
{
    public class RankDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankName;
        [SerializeField] private TextMeshProUGUI count;

        public void Start()
        {
            var score = Game.Game.instance.levelMaxCombo;
            count.text = $"{score}";
            rankName.text = $"Rank {Game.Game.instance.currentLevel.GetRank(score).name}";
        }
    }
}