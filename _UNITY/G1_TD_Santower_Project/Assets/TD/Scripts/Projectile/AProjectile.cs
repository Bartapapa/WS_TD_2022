namespace GSGD1
{
	using System.Runtime.CompilerServices;
	using UnityEngine;

	public abstract class AProjectile : MonoBehaviour
	{
		[SerializeField]
		private bool _destroyIfGiveDamage = true;

		[SerializeField]
		private bool _needDamageableToCollide = true;

		[SerializeField]
		private int _damage = 1;

		[SerializeField]
		protected float _projectileSpeed = 1f;

		[SerializeField]
		protected bool _canFreeze = false;

		[SerializeField]
		protected bool _applyDamage = true;

        [SerializeField]
        private bool _followTarget = false;

        [SerializeField]
        private float _rotationSpeed = 10;

        private Damageable _target;

        [Header("Artillery")]
		[SerializeField]
		protected bool _useArtilleryMovement = false;
		[SerializeField]
		protected float _maximumProjectileHeight = 10f;
		[SerializeField]
		protected LayerMask _enemyLayer;
		protected bool _isMoving = false;

		protected bool _hitThing = false;

		protected Collider _collider;



		public Collider GetCollider => _collider;

		public int Damage { set { _damage = value; } }
		public bool GetHit { get { return _hitThing; } }

        public bool FollowTarget { get => _followTarget; set => _followTarget = value; }
        public Damageable Target { get => _target; set => _target = value; }
        public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }

        public virtual void OnTriggerEnter(Collider other)
		{
			if (_applyDamage == true)
			{
				_collider = other;
				var damageable = other.GetComponentInParent<Damageable>();

				if (damageable != null && _needDamageableToCollide == true)
				{
					if (_canFreeze == true)
					{
						other.GetComponentInParent<Freezer>().Freeze(9999);
					}
					_hitThing = true;
					damageable.TakeDamage(_damage, false);

					if (_destroyIfGiveDamage == true)
					{
						Destroy(gameObject);
					}
				}
				else if (_needDamageableToCollide == false)
				{
					_hitThing = true;
					if (damageable != null)
					{
						damageable.TakeDamage(_damage, false);
					}
				}
			}
		}

		public void SetDamage(int value)
		{
			_damage = value;
		}

		public void SetProjectileSPeed(float value)
		{
			_projectileSpeed = value;
		}
	}
}