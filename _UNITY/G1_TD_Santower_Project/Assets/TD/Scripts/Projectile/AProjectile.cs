namespace GSGD1
{
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

		[Header("Artillery")]
        [SerializeField]
        protected bool _useArtilleryMovement = false;
        [SerializeField]
        protected float _maximumProjectileHeight = 10f;
        [SerializeField]
        protected LayerMask _enemyLayer;
        protected bool _isMoving = false;

        protected bool _hitThing = false;

		public bool GetHit{ get { return _hitThing; } }

        public virtual void OnTriggerEnter(Collider other)
		{
			var damageable = other.GetComponentInParent<Damageable>();

			if (damageable != null && _needDamageableToCollide == true)
			{

				_hitThing= true;
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