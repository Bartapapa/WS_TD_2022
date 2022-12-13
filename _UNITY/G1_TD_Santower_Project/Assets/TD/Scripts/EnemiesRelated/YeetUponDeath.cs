using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetUponDeath : MonoBehaviour
{
    private Damageable _damageable;

    [SerializeField]
    private float _yeetPercentage = 0f;

    [SerializeField]
    private GameObject _originalObject;

    [Header("Yeeted object")]
    [SerializeField]
    private Rigidbody _rb;

    private bool _hasBeenYeeted = false;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.CallerDied -= Yeet;
        _damageable.CallerDied += Yeet;
    }

    private void OnDisable()
    {
        _damageable.CallerDied -= Yeet;
    }

    public void Yeet(Damageable caller, int currentHealth, int damageTaken)
    {
        if (_hasBeenYeeted)
        {
            return;
        }

        if (_yeetPercentage > 0f)
        {
            float randomFloat = Random.Range(0f, 100f);
            if (randomFloat > _yeetPercentage)
            {
                return;
            }
        }

        Rigidbody newobject = Instantiate(_rb, transform.position, Quaternion.identity);
        Vector3 force = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f));
        float randomPower = Random.Range(10f, 15f);
        newobject.AddForce(force * randomPower, ForceMode.Impulse);
        newobject.AddTorque(force * randomPower, ForceMode.Impulse);
        Destroy(_originalObject);
    }
}
