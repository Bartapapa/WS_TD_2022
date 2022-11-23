namespace GSGD1
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

		public delegate void DamageableEvent(Damageable caller, int currentHealth, int damageTaken);
		private event DamageableEvent _damageTaken = null;

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

		public Vector3 GetAimPosition()
		{
			return _aimPosition.position;
		}

		public void TakeDamage(int damage)
		{
			_health -= damage;

			if (_health <= 0)
			{
				_damageTaken?.Invoke(this, _health, damage);

				if (_destroyIfKilled == true)
				{
					DoDestroy();
				}
			}
		}

		public bool GetIsFlying { get { return _isFlying; } }
		public int GetHealth { get { return _health; } }

		private void DoDestroy()
		{
			// A remplacer par les scripts / animation de mort
			var particle = Instantiate(_deathParticle);
			particle.transform.position = transform.position;
			Destroy(gameObject);
		}
	}
}