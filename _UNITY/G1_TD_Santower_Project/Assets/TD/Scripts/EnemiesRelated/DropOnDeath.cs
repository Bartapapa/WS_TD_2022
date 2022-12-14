using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOnDeath : MonoBehaviour
{
    [SerializeField] private Damageable _damageable;

    [SerializeField] private int _cookiesDropped = 0;
    [SerializeField] private int _milkDropped = 0;

	private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.CallerDied -= OnCallerDied;
        _damageable.CallerDied += OnCallerDied;
    }

    private void OnDisable()
    {
        _damageable.CallerDied -= OnCallerDied;
    }

    private void OnCallerDied(Damageable caller, int currentHealth, int damageTaken)
	{
		ResourceManager.Instance.AcquireResource(ResourceManager.ResourceType.Cookie, _cookiesDropped);
		ResourceManager.Instance.AcquireResource(ResourceManager.ResourceType.Milk, _milkDropped);
	}
}
