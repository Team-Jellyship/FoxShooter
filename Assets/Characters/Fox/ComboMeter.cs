using System;
using FoxShooter.Game;
using FoxShooter.Game.Combo;
using UnityEngine;
using UnityEngine.Events;

namespace FoxShooter.Characters.Fox
{
    public class ComboMeter : MonoBehaviour
    {
        [Header("Combo Rank")]
        [field: SerializeField] public float maxCombo { get; private set; } = 100.0f;
        [SerializeField] private float comboDecayRate = 1.0f;
        [SerializeField] private ComboTable combos;
        
        [Header("Kills")]
        [SerializeField] private float killTime = 2.0f;
        
        public UnityEvent<float> comboValueChanged;
        public UnityEvent<int> killCountChanged;
        public UnityEvent<int> killComboEnded; // The combo's value when it ended
        
        public float currentComboValue { get; private set; }
        public int killCount { get; private set; }
        
        private int _currentRankIndex;
        
        private TimerHandle _killComboTimer;

        private void Start()
        {
            _killComboTimer = TimerManager.instance.CreateTimer(this, EndKillCombo);
        }
        
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

        public void IncreaseKillCount()
        {
            ++killCount;
            killCountChanged.Invoke(killCount);
            _killComboTimer.Start(killTime);
        }

        public float GetKillTimerPercentage()
        {
            return Mathf.Clamp(_killComboTimer.GetRemainingTime() / killTime, 0.0f, 1.0f);
        }

        private void EndKillCombo()
        {
            Game.Game.instance.levelMaxCombo = Math.Max(Game.Game.instance.levelMaxCombo, killCount);
            killComboEnded.Invoke(killCount);
            killCount = 0;
            killCountChanged.Invoke(0);
        }
    }
}