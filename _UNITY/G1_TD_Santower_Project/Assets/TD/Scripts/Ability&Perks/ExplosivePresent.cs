using GSGD1;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class ExplosivePresent : MonoBehaviour, IPickerGhost
{
    [SerializeField]
    private ProjectileExplosive _explosion;

    [SerializeField]
    private float _explosionRadius;

    [SerializeField]
    private int _explosionDamage;

    [Header("Present parameters")]
    [SerializeField]
    private float _projectileSpeed = 1f;

    private bool _isMoving = false;

    private bool _canExplode = false;

    [SerializeField]
    private AnimationCurve _curve;

    [SerializeField]
    private float _presentMaxHeight = 1f;

    [SerializeField]
    private Timer _activationTimer;

    [SerializeField]
    private AnimatorHandler _anim;

    private void Update()
    {
        _activationTimer.Update();
        if (_activationTimer.Progress >= 1)
        {
            SetCanExplode(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_canExplode) return;

        WaveEntity entity = other.GetComponent<WaveEntity>();
        if (entity != null)
        {

            _canExplode = false;
            ProjectileExplosive newExplosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            newExplosion.ExplosionRadius = _explosionRadius;
            newExplosion.Damage = _explosionDamage;
            _anim.Animator.SetTrigger("Die");
        }
    }

    public void ExplodePresent()
    {
        //Replace with anims and such.
        Destroy(this.gameObject);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SetCanExplode(bool value)
    {
        _canExplode = value;
    }

    public void ThrowPresent(Vector3 targetPosition, Vector3 direction)
    {
        transform.LookAt(targetPosition);
        StartCoroutine(ArcPresent(targetPosition));
    }

    IEnumerator ArcPresent(Vector3 targetPosition)
    {
        _isMoving = true;
        float time = 0f;
        Vector3 startPosition = transform.position;

        Vector3 destination = targetPosition;

        float duration = 60f / _projectileSpeed;

        while (time < duration)
        {
            time += Time.deltaTime;

            float linearT = time / duration;
            float heightT = _curve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, _presentMaxHeight, heightT);

            transform.position = Vector3.Lerp(startPosition, destination, linearT) + new Vector3(0f, height);

            yield return null;
        }

        _isMoving = false;
        PresentLand();
        yield return null;
    }

    private void PresentLand()
    {
        _anim.Animator.SetTrigger("Landed");
        _activationTimer.Start();
    }

    public void SetExplosionRadius(float radius)
    {
        _explosionRadius = radius;
    }

    public void SetExplosionDamage(int damage)
    {
        _explosionDamage = damage;
    }
}
