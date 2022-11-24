using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
	[SerializeField]
	private bool _destroyIfGiveDamage = true;

	[SerializeField]
	private int _damage = 1;

	protected virtual void OnTriggerEnter(Collider other)
	{
		var damageable = other.GetComponentInParent<Damageable>();

		if (damageable != null)
		{
			damageable.TakeDamage(_damage);

			if (_destroyIfGiveDamage == true)
			{
				Destroy(transform.parent.gameObject);
			}
		}
	}
}
