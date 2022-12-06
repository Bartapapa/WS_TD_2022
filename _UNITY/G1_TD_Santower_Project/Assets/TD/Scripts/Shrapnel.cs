using Cinemachine.Utility;
using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Shrapnel : MonoBehaviour
{
    [SerializeField]
    private ProjectileExplosive _projExplosive;

    [SerializeField]
    private ProjectileExplosive _shrapnel;
    [SerializeField]
    private Transform _shrapnelSpawner;
    [SerializeField]
    private int _shrapnelQuantity = 5;
    [SerializeField]
    private float _spread = 15f;


    private void Awake()
    {
        _projExplosive = GetComponent<ProjectileExplosive>();
    }

    private void OnEnable()
    {
        _projExplosive.Exploded -= OnExploded;
        _projExplosive.Exploded += OnExploded;
    }

    private void OnDisable()
    {
        _projExplosive.Exploded -= OnExploded;
    }

    private void OnExploded()
    {
        Debug.Log("!");
        if (!_projExplosive.CanProduceShrapnel) return;
        StartCoroutine(SendShrapnel());
    }

    private IEnumerator SendShrapnel()
    {
        int spawnedShrapnel = 0;
        while (spawnedShrapnel < _shrapnelQuantity)
        {
            spawnedShrapnel += 1;
            Vector2 randomSpread = new Vector2(Random.Range(-_spread, _spread), Random.Range(-_spread, _spread));
            Quaternion spreadQuat = Quaternion.Euler((randomSpread.x / 4), randomSpread.y, 1);
            ProjectileExplosive newProjectile;
            newProjectile = Instantiate(_shrapnel, _shrapnelSpawner.transform.position, _shrapnelSpawner.transform.rotation * spreadQuat);
            yield return null;
        }
        yield return null;
    }
}
