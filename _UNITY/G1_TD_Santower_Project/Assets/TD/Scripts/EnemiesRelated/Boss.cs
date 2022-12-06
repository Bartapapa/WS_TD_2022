using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	[SerializeField]
	private GameManager.GamePhase _gamePhase;

	private GameManager _gameManager;

	private Damageable _damageable;

	private void OnEnable()
	{
		_damageable.CallerDied -= Die;
		_damageable.CallerDied += Die;
	}

	private void OnDisable()
	{
		_damageable.CallerDied -= Die;
	}

	private void Awake()
	{
		_gameManager = GameManager.Instance;
		_damageable = gameObject.GetComponent<Damageable>();
	}

	private void Die(Damageable damageable, int currentHealth, int damage)
	{
		if (_gameManager != null)
		{
			_gameManager.ChangePhase(_gamePhase);
		}
	}

}
