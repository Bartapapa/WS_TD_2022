using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseNorthPole : MonoBehaviour
{
	[SerializeField]
	private Damageable _damageable;

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
		if (_damageable.GetHealth <= 0)
		{
			// Insert Code for defeat screen
			QuitToEditor();
		}
	}

	private void OnNorthPoleBombed(Damageable caller, int currentHealth, int damageTaken)
	{
		EnemyBombed.Invoke(caller);
	}

	private void QuitToEditor()
	{
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
