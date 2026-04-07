using FoxShooter.Game.StatusEffects;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FoxShooter.Characters
{
    public class CharacterStats : MonoBehaviour
    {
		public UnityEvent onDeath;
		public UnityEvent<float> onTakeDamage;
		public UnityEvent onStunned;
		public UnityEvent onStunEnd;


		public bool canBeJumpedOn { get; }
		public Team characterTeam { private set; get; }
		[SerializeField] private float defaultHealth = 15;
		[SerializeField] private float invulnerabilityTime;
		
		
		private bool _showingStats;
		private readonly StatusEffectContainer _effects = new();
		
		public void Start()
		{
			_effects.SetBaseValue(Game.Game.instance.statusEffects.health, defaultHealth);
			_effects.SetBaseValue(Game.Game.instance.statusEffects.maxHealth, defaultHealth);
			
			_effects.RegisterEffectAppliedCallback(Game.Game.instance.statusEffects.invulnerability, onStunned.Invoke, this);
			_effects.RegisterEffectRemovedCallback(Game.Game.instance.statusEffects.invulnerability, onStunEnd.Invoke, this);
		}

		public void Update()
		{
			_effects.Update(Time.deltaTime);
		}

		/**
		 * <summary>
		 *     Take damage. Automatically emits events associated with the health effect,
		 *     and triggers the OnDeath signal if the damage causes this character to die
		 * </summary>
		 */
		public virtual void TakeDamage(float damageAmount, CharacterStats source, bool canBeBlocked)
		{
			if (_effects.HasEffect(Game.Game.instance.statusEffects.invulnerability))
			{
				return;
			}
			
			Debug.Log($"[CharacterStats] '{gameObject.name}' took '{damageAmount}' damage.");
			onTakeDamage.Invoke(damageAmount);
			var healthEffect = Game.Game.instance.statusEffects.health;
			if (!(_effects.AddBaseValue(healthEffect, -damageAmount) <= 0.0f))
			{
				if (invulnerabilityTime > 0.0f)
				{
					_effects.ApplyStatusEffectInstance(new StatusEffectInstance(Game.Game.instance.statusEffects.invulnerability, this, invulnerabilityTime));
				}
				return;
			}
			Kill(source);
		}

		public void Kill(CharacterStats source)
		{
			_effects.SetBaseValue(Game.Game.instance.statusEffects.health, 0.0f);
			source?.KilledEnemy(this);
			onDeath.Invoke();
		}

		public virtual void KilledEnemy(CharacterStats enemy)
		{
		}

		public void RegisterEffectChangedDelegate(StatusEffect effect, UnityAction<int, float> action, MonoBehaviour owner)
		{
			_effects.RegisterStatusEffectChangedEvent(effect, action, owner);
		}

		public void RegisterEffectAppliedCallback(StatusEffect effect, UnityAction action, MonoBehaviour owner)
		{
			_effects.RegisterEffectAppliedCallback(effect, action, owner);
		}

		public void RegisterEffectRemovedCallback(StatusEffect effect, UnityAction action, MonoBehaviour owner)
		{
			_effects.RegisterEffectRemovedCallback(effect, action, owner);
		}

		public float GetEffectValue(StatusEffect effect)
		{
			return _effects.GetValue(effect);
		}

		public void ApplyStatusEffect(StatusEffectInstance statusEffectInstance)
		{
			_effects.ApplyStatusEffectInstance(statusEffectInstance);
		}

		public void ApplyStatusEffect(StatusEffect effect, MonoBehaviour owner)
		{
			ApplyStatusEffect(new StatusEffectInstance(effect, owner));
		}

		public void RemoveStatusEffectInstance(StatusEffectInstance instance)
		{
			_effects.RemoveStatusEffectInstance(instance);
		}
    }
}