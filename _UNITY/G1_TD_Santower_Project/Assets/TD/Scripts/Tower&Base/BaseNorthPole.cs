using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseNorthPole : MonoBehaviour
{
	[SerializeField]
	private Damageable _damageable;

	[SerializeField]
	private Slider _health;

    public UnityEvent<Damageable> EnemyBombed = null;

    private void OnEnable()
    {
		_damageable.NorthPoleBombed -= OnNorthPoleBombed;
        _damageable.NorthPoleBombed += OnNorthPoleBombed;


        EnemyBombed.RemoveListener(LevelReferences.Instance.SpawnerManager.OnEntityReachedBase);
        EnemyBombed.AddListener(LevelReferences.Instance.SpawnerManager.OnEntityReachedBase);
    }

    private void OnDisable()
    {
        _damageable.NorthPoleBombed -= OnNorthPoleBombed;

        EnemyBombed.RemoveListener(LevelReferences.Instance.SpawnerManager.OnEntityReachedBase);
    }


    private void Update()
	{
		if (_health != null)
		{
            _health.value = _damageable.GetHealth;
        }
	}

	private void OnNorthPoleBombed(Damageable caller, int currentHealth, int damageTaken)
	{
		EnemyBombed.Invoke(caller);
	}
}
