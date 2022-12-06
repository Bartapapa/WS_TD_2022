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

	[SerializeField]
	private Damageable _damageable;

	private void Awake()
	{
		_damageable = GetComponent<Damageable>();
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		var damageable = other.GetComponentInParent<Damageable>();

		if (damageable != null && other.GetComponentInParent<Tower>() == false)
		{
			damageable.TakeDamage(_damage, true);

			if (_dieIfGiveDamage == true)
			{
				if (_damageable != null)
				{
                    _damageable.Die();
                }
				
			}
		}
	}
}
