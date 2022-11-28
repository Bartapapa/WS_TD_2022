﻿namespace GSGD1
{
	using UnityEngine;

	public class Damageable : MonoBehaviour
	{
		[SerializeField]
		private bool _isFlying = false;

		[SerializeField]
		private int _health = 1;

		[SerializeField]
		private bool _destroyIfKilled = true;

		[SerializeField]
		private Transform _aimPosition = null;

		[SerializeField]
		private ParticleSystem _deathParticle = null;

		[SerializeField]
		private bool _isDead = false;
		[SerializeField]
		private bool _isInvulnerable = false;

		[SerializeField]
		private Timer _deathTimer;
		[SerializeField]
		private Timer _invulnerabilityTimer;

		public bool IsDead => _isDead;

		public delegate void DamageableEvent(Damageable caller, int currentHealth, int damageTaken);
		private event DamageableEvent _damageTaken = null;
		private event DamageableEvent _callerDied = null;

		private void Awake()
		{
			Invulnerability(.1f);
		}

		public event DamageableEvent DamageTaken
		{
			add
			{
				_damageTaken -= value;
				_damageTaken += value;
			}
			remove
			{
				_damageTaken -= value;
			}
		}

        public event DamageableEvent CallerDied
        {
            add
            {
                _callerDied -= value;
                _callerDied += value;
            }
            remove
            {
                _callerDied -= value;
            }
        }

		private void Update()
		{
			_deathTimer.Update();
			_invulnerabilityTimer.Update();

			if(_invulnerabilityTimer.Progress >= 1f)
			{
				_isInvulnerable = false;
			}

			if (_deathTimer.Progress >= 1f)
			{
				DoDestroy();
			}
		}

		public Vector3 GetAimPosition()
		{
			if (_aimPosition != null)
			{
				return _aimPosition.position;
			}
			else
				return transform.position;
		}

		public void TakeDamage(int damage)
		{
			if (_isInvulnerable) return;

			_health -= damage;

            _damageTaken?.Invoke(this, _health, damage);

            if (_health <= 0)
			{
				Die();
			}
		}

		public bool GetIsFlying { get { return _isFlying; } }
		public int GetHealth { get { return _health; } }

		public void DoDestroy()
		{
			if (_destroyIfKilled == true)
			{
				Destroy(gameObject);
			}
		}

		public void Die()
		{
			_callerDied?.Invoke(this, _health, 0);

            // A remplacer par les scripts / animation de mort
			if (_deathParticle != null)
			{
                var particle = Instantiate(_deathParticle);
                particle.transform.position = transform.position;
            }

			_isDead = true;
			DoDestroy();
        }

		private void Invulnerability(float duration)
		{
            _invulnerabilityTimer.Set(duration);
			_invulnerabilityTimer.Start();
			_isInvulnerable = true;
        }
	}
}