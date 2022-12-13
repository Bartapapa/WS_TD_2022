using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemies : MonoBehaviour
{
	[SerializeField]
	private Damageable _damageable;

	[SerializeField]
	private ProjectileExplosive _explosion;

	[SerializeField]
	private float _explosionRadius = 3;

	[SerializeField]
	private float _explosionSpeed = 100;

	[SerializeField]
	private bool _explodeImmediatelyOnDeath = true;

	private void OnEnable()
	{
		_damageable.CallerDied -= OnCallerDied;
		_damageable.CallerDied += OnCallerDied;
	}

	private void OnDisable()
	{
		_damageable.CallerDied -= OnCallerDied;
	}

	private void OnCallerDied(Damageable damageable, int currentHealth, int damageTaken)
	{
		if (!_explodeImmediatelyOnDeath) return;
		Explosion();

	}

	public void Explosion()
	{
        _explosion.ExplosionRadius = _explosionRadius;
        _explosion.ExplosionSpeed = _explosionSpeed;
        Instantiate(_explosion, transform.position, Quaternion.identity);
    }


}
