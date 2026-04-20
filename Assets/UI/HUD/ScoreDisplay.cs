using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace FoxShooter.UI.HUD
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreDisplay : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        private void Start()
        {
            Game.Game.instance.scoreChanged.AddListener(SetScoreDisplay);
        }

        private void OnDestroy()
        {
            Game.Game.instance.scoreChanged.RemoveListener(SetScoreDisplay);
        }

        private void SetScoreDisplay(float score)
        {
            _text.text = $"{score}";
        }
    }
}