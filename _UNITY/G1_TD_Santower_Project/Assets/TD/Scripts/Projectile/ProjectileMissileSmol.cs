using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissileSmol : AProjectile
{
    public delegate void ExplosiveEvent();
    public event ExplosiveEvent Exploded = null;
    
    [Header("Missile")]
    [SerializeField]
    private SphereCollider _explosionCollider;

    [SerializeField]
    private float _explosionRadius = 3;

    [SerializeField]
    private float _rotationSpeed = 10;

    private float _explosionSpeed = 33;

    private bool _hasExploded = false;

    private bool _canMove = true;

    private Damageable _target;

    [System.NonSerialized]
    private Quaternion _lastLookRotation = Quaternion.identity;

    public float ExplosionRadius { set => _explosionRadius = value; }
    public float ExplosionSpeed { set => _explosionSpeed = value; }

    public Damageable Target { set => _target = value; }

    private void Start()
    {
        ExplosionSpeed = _explosionSpeed * _explosionRadius;
    }

    private void Update()
    {
        if (_canMove == true)
        {
            MoveForward();
        }
        if (GetHit == true)
        {
            EXPLOSION();
        }
        LookAt(_target.GetAimPosition());
    }

    private void MoveForward()
    {

        transform.position = transform.position + _projectileSpeed * Time.deltaTime * transform.forward;
    }

    private void EXPLOSION()
    {
        _projectileSpeed = 0;
        _explosionCollider.radius = _explosionCollider.radius + _explosionSpeed * Time.deltaTime;

        if (!_hasExploded)
        {
            _hasExploded = true;
            Exploded?.Invoke();
        }

        if (_explosionCollider.radius >= _explosionRadius)
        {
            Destroy(gameObject);
        }
    }

    public virtual void LookAt(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;

        _lastLookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lastLookRotation, _rotationSpeed * Time.deltaTime);

        AnchorLookAt(position);
    }
    public void AnchorLookAt(Vector3 position)
    {
        //TODO force anchor to follow WeaponController.forward.

        Vector3 lookPos = position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        transform.rotation = lookRotation;
    }
}
