using System;
using FoxShooter.Game;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace FoxShooter.UI.HUD
{
    public class LevelDefinitionDisplay : MonoBehaviour
    {
        [SerializeField] private LevelDefinition levelDefinition;
        
        [Header("Elements")]
        [SerializeField] private TextMeshProUGUI rankLabel;
        [SerializeField] private TextMeshProUGUI levelNameLabel;
        [SerializeField] private LevelSelectButton clickableThumbnail;

        private void Start()
        {
            if (!levelDefinition)
            {
                return;
            }

            var bestRank = levelDefinition.GetBestRank();
            rankLabel.text = bestRank.name;
            levelNameLabel.text = levelDefinition.displayName;
            clickableThumbnail.level = levelDefinition;
        }
    }
}