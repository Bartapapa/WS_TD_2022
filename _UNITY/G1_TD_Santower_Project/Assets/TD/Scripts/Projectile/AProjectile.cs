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

		private bool _hitThing = false;

		public bool GetHit{ get { return _hitThing; } }

		public virtual void OnTriggerEnter(Collider other)
		{
			var damageable = other.GetComponentInParent<Damageable>();

			if (damageable != null && _needDamageableToCollide == true)
			{
				_hitThing= true;
				damageable.TakeDamage(_damage);

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
					damageable.TakeDamage(_damage);
				}
			}
		}
	}
}