using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnZone : AProjectile
{
	[SerializeField]
	private Timer _timer;

	[SerializeField]
	private bool _teleportOnTarget = false;

	private List<Damageable> _damageable = new List<Damageable>();

	private Lifespan _lifespan;

	private DamageableDetector _damageableDetector;

	private void Awake()
	{
		_damageableDetector = GetComponent<DamageableDetector>();
		_lifespan = GetComponent<Lifespan>();
	}

	private void OnEnable()
	{
		_timer.Start();
		
	}

	private void Update()
	{
		if (_teleportOnTarget == true && Target != null)
		{
			transform.position = Target.transform.position;
		}
		
		_timer.Update();
		_damageableDetector.RemoveNullItemsFromList();
		
		if (_timer.Progress >= 1)
		{
			_damageable = _damageableDetector.DamageablesInRange;
			foreach (Damageable damageable in _damageable)
			{
				damageable.TakeDamage(Damage, false);
				_damageableDetector.RemoveNullItemsFromList();
			}
			_timer.Update();

		}
	}
}
