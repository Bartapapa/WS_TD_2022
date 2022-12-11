using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Napalm : MonoBehaviour
{
	[SerializeField]
	private Timer _timer;

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
		_damageable = _damageableDetector.DamageablesInRange;
		_timer.Update();
		if (_timer.Progress >= 1)
		{
			_timer.Update();
			foreach (Damageable damageable in _damageable)
			{
				damageable.TakeDamage(1, false);
			}

		}
	}
}
