using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using FoxShooter.Characters.AI.Polestar;
using FoxShooter.Characters;
using FoxShooter.Game;
using FoxShooter.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;

namespace Characters.AI.Polestar
{
    public class PolestarDebugger : MonoBehaviour
    {
        [SerializeField] private PolestarStack _stack;
        [SerializeField] private CharacterStats _self;
        [SerializeField] private CharacterStats _target;
        [SerializeField] [Min(0.0f)] private float _interval = 5.0f;

        private TimerHandle _queryTimer;

        private List<PolestarResult> _results = new();

        private void Start()
        {
            _queryTimer = TimerManager.instance.CreateTimer(this, RunQuery);
            _queryTimer.Start(_interval, true);
        }

        public void RunQuery()
        {
            if (!_stack)
            {
                return;
            }

            _results = _stack.Evaluate(_self, _target);
        }

        public void ClearQuery()
        {
            _results.Clear();
        }

        private void OnDrawGizmos()
        {
            if (_results.Count == 0)
            {
                return;
            }
            
            var quat = Quaternion.FromToRotation(Vector3.forward, Vector3.down);
            var max = PolestarResult.Max(ref _results);
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 36,
                normal =
                {
                    textColor = Color.black
                }
            };
            
            foreach (var result in _results)
            {
                Handles.Label(result.position, $"{result.score:0.00}", style);
                var color = result == _results[max] ? Color.dodgerBlue : new Color(result.score, 0.0f, 0.0f);
                StarDebug.DrawCircle(result.position, quat, 1.0f, color);
            }
            Handles.color = Color.white;
        }
    }
}