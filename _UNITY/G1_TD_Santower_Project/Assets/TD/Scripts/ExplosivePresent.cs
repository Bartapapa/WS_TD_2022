using GSGD1;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private List<GameObject> _colorVariants = new List<GameObject>();

    [Header("Present parameters")]
    [SerializeField]
    private float _projectileSpeed = 1f;

    private bool _isMoving = false;

    private bool _canExplode = false;

    [SerializeField]
    private Timer _activationTimer;

    private void Awake()
    {
        foreach(GameObject variant in _colorVariants)
        {
            variant.SetActive(false);
        }
        int randomVariant = Random.Range(0, _colorVariants.Count);
        _colorVariants[randomVariant].SetActive(true);
    }

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
            ProjectileExplosive newExplosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            newExplosion.ExplosionRadius = _explosionRadius;
            newExplosion.Damage = _explosionDamage;
            ExplodePresent();
        }
    }

    private void ExplodePresent()
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
        StartCoroutine(ArcPresent(targetPosition, direction));
    }

    IEnumerator ArcPresent(Vector3 targetPosition, Vector3 direction)
    {
        _isMoving = true;
        Vector3 destinationPoint = targetPosition;
        Debug.Log(destinationPoint);

        float travelDistance = Vector3.Distance(transform.position, destinationPoint);
        float halfTravelDistance = travelDistance * .5f;
        float distanceTravelled = 0f;
        Vector3 currentPos = transform.position;
        float distanceRatio = 0f;

        while (distanceTravelled < halfTravelDistance)
        {
            transform.position += (transform.forward * _projectileSpeed * Time.deltaTime);
            distanceTravelled += Vector3.Distance(currentPos, transform.position);
            currentPos = transform.position;

            distanceRatio = 1 - (distanceTravelled / halfTravelDistance);
            float rise = _projectileSpeed * distanceRatio * Time.deltaTime;
            transform.position += new Vector3(0, rise, 0);

            yield return null;
        }
        distanceTravelled = 0f;
        while (distanceTravelled < halfTravelDistance)
        {
            transform.position += (transform.forward * _projectileSpeed * Time.deltaTime);
            distanceTravelled += Vector3.Distance(currentPos, transform.position);
            currentPos = transform.position;

            distanceRatio = distanceTravelled / halfTravelDistance;
            float fall = _projectileSpeed * distanceRatio * Time.deltaTime;
            transform.position += new Vector3(0, -fall, 0);

            yield return null;
        }
        transform.position = destinationPoint;
        _isMoving = false;
        PresentLand();
        yield return null;
    }

    private void PresentLand()
    {
        transform.LookAt(Vector3.right);
        _activationTimer.Start();
    }
}
