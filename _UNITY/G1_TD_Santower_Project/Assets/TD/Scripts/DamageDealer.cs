using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
	[SerializeField]
	private bool _dieIfGiveDamage = true;

	[SerializeField]
	private int _damage = 1;

	protected virtual void OnTriggerEnter(Collider other)
	{
		var damageable = other.GetComponentInParent<Damageable>();

		if (damageable != null)
		{
			damageable.TakeDamage(_damage);

			if (_dieIfGiveDamage == true)
			{
				//A remplacer par Die() du Damageable!
				damageable.Die();
			}
		}
	}
}
