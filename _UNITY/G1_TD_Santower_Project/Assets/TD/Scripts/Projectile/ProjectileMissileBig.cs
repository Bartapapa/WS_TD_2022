using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissileBig : AProjectile
{
    public delegate void ExplosiveEvent();
    public event ExplosiveEvent Exploded = null;

    private List<Damageable> _damageables= new List<Damageable>();

	[SerializeField]
	private DamageableDetector _damageableDetector;

	[Header("Missile")]
    [SerializeField]
    private float _heightExplosion = 6.5f;

    [SerializeField]
    private ProjectileMissileSmol _smolMissile;

    [SerializeField]
    private int _smolMissileSpawn = 5;

    [SerializeField]
    private float _radiusSpawnSmolMissile = 1;

    [SerializeField]
    private SphereCollider _explosionCollider;

    [SerializeField]
    private float _explosionRadius = 3;

    private float _explosionSpeed = 33;
   
    private bool _hasExploded = false;

    private bool _canMove = true;

    public float ExplosionRadius { set => _explosionRadius = value; }
    public float ExplosionSpeed { set => _explosionSpeed = value; }

    private void Start()
    {
        ExplosionSpeed = _explosionSpeed * _explosionRadius;
    }

    private void Update()
    {
        if(_canMove == true)
        {
            MoveForward();
        }
        if (transform.position.y >= _heightExplosion)
        {
            _canMove = false;
            EXPLOSION();
        }
    }

    private void MoveForward()
    {

        transform.position = transform.position + _projectileSpeed * Time.deltaTime * transform.forward;
    }

    private void EXPLOSION()
    {
        _applyDamage= true;
        _projectileSpeed = 0;
        _explosionCollider.radius = _explosionCollider.radius + _explosionSpeed * Time.deltaTime;

        if (!_hasExploded)
        {
            _hasExploded = true;
            Exploded?.Invoke();
        }

        if (_explosionCollider.radius >= _explosionRadius)
        {
            SpawnMissiles();
            Destroy(gameObject);
        }
    }

    private void SpawnMissiles()
    {
       _damageables = _damageableDetector.DamageablesInRange;
        for (int i = 0; i < _smolMissileSpawn; i++)
        {
            if (_damageables.Count > 0)
            {
                ProjectileMissileSmol missile = Instantiate(_smolMissile, 
                                                            transform.position + new Vector3 (
                                                            Random.Range(-_radiusSpawnSmolMissile, _radiusSpawnSmolMissile), 
                                                            Random.Range(-_radiusSpawnSmolMissile, _radiusSpawnSmolMissile), 
                                                            Random.Range(-_radiusSpawnSmolMissile, _radiusSpawnSmolMissile)), 
                                                            Quaternion.identity);
                missile.Target = _damageables[Random.Range(0, _damageables.Count - 1)];
            }
        }
    }
}
