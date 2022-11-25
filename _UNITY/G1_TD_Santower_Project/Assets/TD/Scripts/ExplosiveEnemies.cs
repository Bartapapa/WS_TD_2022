using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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

	private void OnEnable()
	{
		_damageable.CallerDied -= Explosion;
		_damageable.CallerDied += Explosion;
	}

	private void OnDisable()
	{
		_damageable.CallerDied -= Explosion;
	}

	private void Explosion(Damageable damageable, int currentHealth, int damageTaken)
	{
		Instantiate(_explosion, transform.position, Quaternion.identity);
	}
}
