using System;
using FoxShooter.Game;
using FoxShooter.Game.GamemodeGraph.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace FoxShooter.UI.HUD
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class LevelSelectButton : MonoBehaviour
    {
        public LevelDefinition level
        {
            get => _level;
            set
            {
                _level = value;
                _image.sprite = _level ? _level.thumbnail : null;
            }
        }
        
        private LevelDefinition _level;
        
        private Button _button;
        private Image _image;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SelectLevel);
        }

        private void SelectLevel()
        {
            if (!level)
            {
                return;
            }
            
            Game.Game.instance.nextLevel = level;
            Game.Game.instance.Command(GamemodeTransitionFlag.Advance);
        }
    }
}