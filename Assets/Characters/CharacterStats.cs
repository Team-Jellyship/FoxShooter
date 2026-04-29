using FoxShooter.Game;
using FoxShooter.Game.StatusEffects;
using FoxShooter.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FoxShooter.Characters
{
	public enum DamageType
	{
		Unaspected,
		Gunfire,
		Melee
	}
	
    public class CharacterStats : MonoBehaviour
    {
		public UnityEvent onDeath;
		public UnityEvent<float> onTakeDamage;
		public UnityEvent<float> onHeal;
		public UnityEvent onStunned;
		public UnityEvent onStunEnd;
		public UnityEvent<CharacterStats, DamageType> onKillCharacter;


		public bool alive { private set; get;  } = true;
		[field: SerializeField] public Team characterTeam { private set; get; }
		[SerializeField] private float defaultHealth = 15;
		[SerializeField] private float invulnerabilityTime;
		[SerializeField] private float despawnTime = 0.5f;
		[SerializeField] private float score;
		[SerializeField] private Animator _animator; // For Animation Triggers
		
		private bool _showingStats;
		private TimerHandle _despawnTimer;
		private readonly StatusEffectContainer _effects = new();
		
		public void Start()
		{
			_effects.SetBaseValue(Game.Game.instance.statusEffects.health, defaultHealth);
			_effects.SetBaseValue(Game.Game.instance.statusEffects.maxHealth, defaultHealth);
			
			_effects.RegisterEffectAppliedCallback(Game.Game.instance.statusEffects.invulnerability, onStunned.Invoke, this);
			_effects.RegisterEffectRemovedCallback(Game.Game.instance.statusEffects.invulnerability, onStunEnd.Invoke, this);

			_animator = GetComponent<Animator>();
			_despawnTimer = TimerManager.instance.CreateTimer(this, () => Destroy(gameObject));
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
		public virtual void TakeDamage(float damageAmount, CharacterStats source, bool canBeBlocked, DamageType type = DamageType.Unaspected)
		{
			if (_effects.HasEffect(Game.Game.instance.statusEffects.invulnerability))
			{
				return;
			}

			if (_effects.GetValue(Game.Game.instance.statusEffects.health) == 0.0f)
			{
				return;
			}
			
			Debug.Log($"[CharacterStats] '{gameObject.name}' took '{damageAmount}' damage from '{StarNames.GetNameSafe(source)}'.");
			onTakeDamage.Invoke(damageAmount);
			_animator.SetTrigger("Damage");
			var healthEffect = Game.Game.instance.statusEffects.health;
			if (!(_effects.AddBaseValue(healthEffect, -damageAmount) <= 0.0f))
			{
				if (invulnerabilityTime > 0.0f)
				{
					_effects.ApplyStatusEffectInstance(new StatusEffectInstance(Game.Game.instance.statusEffects.invulnerability, this, invulnerabilityTime));
				}
				return;
			}
			Kill(source, type);
		}

		public void Heal(float amount)
		{
			if (amount < 0.0f)
			{
				return;
			}
			
			Debug.Log($"[CharacterStats] '{gameObject.name} healed by '{amount}'.");
			onHeal.Invoke(amount);
			
			var healthEffect = Game.Game.instance.statusEffects.health;
			var maxHealthEffect = Game.Game.instance.statusEffects.maxHealth;
			var maxHealth = GetEffectValue(maxHealthEffect);
			
			// Cap health
			if (_effects.AddBaseValue(healthEffect, amount) > maxHealth)
			{
				_effects.SetBaseValue(healthEffect, maxHealth);
			}
		}

		public virtual void Kill(CharacterStats source, DamageType type = DamageType.Unaspected)
		{
			_effects.SetBaseValue(Game.Game.instance.statusEffects.health, 0.0f);
			if (source != this)
			{
				source?.KilledEnemy(this, type);
			}
			onDeath.Invoke();

			Game.Game.instance.Score(score);
			if (despawnTime > 0.0f)
			{
				_despawnTimer.Start(despawnTime);
			}
			alive = false;
		}

		// ReSharper disable Unity.PerformanceAnalysis
		protected virtual void KilledEnemy(CharacterStats enemy, DamageType type)
		{
			Debug.Log($"[CharacterStats] '{gameObject.name}' killed enemy '{enemy.gameObject.name}'");
			onKillCharacter.Invoke(enemy, type);
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