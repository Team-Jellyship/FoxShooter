using System;
using System.Collections.Generic;
using System.Linq;
using FoxShooter.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace FoxShooter.Game.StatusEffects
{
    public class StatusEffectContainer
    {
        private readonly Dictionary<StatusEffect, StatusEntry> _statusEffects = new ();

        private readonly SparseEventMap<StatusEffect> _onStatusEffectAppliedMap = new();
        private readonly SparseEventMap<StatusEffect> _onStatusEffectRemovedMap = new();
        private readonly SparseEventMap<StatusEffect, int, float> _onStatusEffectStacksChangedMap = new();

        public void ApplyStatusEffectInstance(StatusEffectInstance instance)
        {
            if (instance.effect == null)
            {
                return;
            }
            
            if (_statusEffects.TryGetValue(instance.effect, out var effectList))
            {
                effectList.Add(instance);
            }
            else
            {
                // Create the stack in our dictionary, and invoke the event for a new stack here
                // if the event exists. Assume no one has subscribed if the event does not exist
                effectList = new StatusEntry(instance.effect, 0.0f) { instance };
                _statusEffects.Add(instance.effect, effectList);
                _onStatusEffectAppliedMap.TriggerEvent(instance.effect);
            }
            
            _onStatusEffectStacksChangedMap.TriggerEvent(instance.effect, effectList.count, effectList.Accumulate());
        }

        public void SetBaseValue(StatusEffect effect, float newBaseValue)
        {
            if (!_statusEffects.TryGetValue(effect, out var effectList))
            {
                effectList = new StatusEntry(effect, newBaseValue);
                _statusEffects.Add(effect, effectList);
                _onStatusEffectStacksChangedMap.TriggerEvent(effect, effectList.count, effectList.Accumulate());
                return;
            }

            // If the old base value is the same, don't signal a change event
            // Comparison check with floating point error
            if (Math.Abs(effectList.baseValue - newBaseValue) < 0.001f)
            {
                return;
            }

            effectList.baseValue = newBaseValue;
            _onStatusEffectStacksChangedMap.TriggerEvent(effect, effectList.count, effectList.Accumulate());
        }

        public void RemoveStatusEffectInstance(StatusEffectInstance instance)
        {
            if (instance.effect == null)
            {
                Debug.LogWarning("[StatusEffectContainer] Attempted to remove a status effect instance, but the effect was null.");
                return;
            }
            
            if (!_statusEffects.TryGetValue(instance.effect, out var effectList))
            {
                return;
            }

            if (!effectList.Remove(instance))
            {
                return;
            }
            
            _onStatusEffectStacksChangedMap.TriggerEvent(instance.effect, effectList.count, effectList.Accumulate());

            if (!effectList.HasExpired())
            {
                return;
            }
            
            _statusEffects.Remove(instance.effect);
            _onStatusEffectRemovedMap.TriggerEvent(instance.effect);
        }

        public void Update(float deltaSeconds)
        {
            var effectsToRemove = new List<StatusEffect>();
            foreach (var stackList in _statusEffects)
            {
                var numRemoved = stackList.Value.RemoveExpired(deltaSeconds);
                if (numRemoved > 0)
                {
                    _onStatusEffectStacksChangedMap.TriggerEvent(stackList.Key, stackList.Value.count, stackList.Value.Accumulate());
                }
                
                if (stackList.Value.HasExpired())
                {
                    effectsToRemove.Add(stackList.Key);
                }
            }

            foreach (var effectToRemove in effectsToRemove)
            {
                _onStatusEffectRemovedMap.TriggerEvent(effectToRemove);
                _statusEffects.Remove(effectToRemove);
            }
        }

        public override string ToString()
        {
            var result = new string("");

            return _statusEffects.Aggregate(result, (current, statusEffect)
                => current + $"{statusEffect.Key.name}: '{statusEffect.Value.count}'");
        }

        public float GetValue(StatusEffect effect)
        {
            return !_statusEffects.TryGetValue(effect, out var effectList) ? 0.0f : effectList.Accumulate();
        }

        public float AddBaseValue(StatusEffect effect, float delta)
        {
            if (_statusEffects.TryGetValue(effect, out var effectList))
            {
                effectList.baseValue += delta;
                var newValue = effectList.Accumulate();
                _onStatusEffectStacksChangedMap.TriggerEvent(effect, effectList.count, newValue);
                return newValue;
            }
            
            SetBaseValue(effect, delta);
            return delta;
        }

        public Dictionary<StatusEffect, StatusEntry>.Enumerator GetEnumerator()
        {
            return _statusEffects.GetEnumerator();
        }

        public void RegisterEffectAppliedCallback(StatusEffect effect, UnityAction action, MonoBehaviour owner)
        {
            _onStatusEffectAppliedMap.RegisterCallback(effect, action);
            owner.destroyCancellationToken.Register(() => _onStatusEffectAppliedMap.RemoveCallback(effect, action));
        }

        public void RegisterEffectRemovedCallback(StatusEffect effect, UnityAction action, MonoBehaviour owner)
        {
            _onStatusEffectRemovedMap.RegisterCallback(effect, action);
            owner.destroyCancellationToken.Register(() => _onStatusEffectRemovedMap.RemoveCallback(effect, action));
        }

        public void RegisterStatusEffectChangedEvent(StatusEffect effect, UnityAction<int, float> action, MonoBehaviour owner)
        {
            _onStatusEffectStacksChangedMap.RegisterCallback(effect, action);
            owner.destroyCancellationToken.Register(() => _onStatusEffectStacksChangedMap.RemoveCallback(effect, action));
        }

        public bool HasEffect(StatusEffect effect)
        {
            return effect != null && _statusEffects.ContainsKey(effect);
        }
    }
}