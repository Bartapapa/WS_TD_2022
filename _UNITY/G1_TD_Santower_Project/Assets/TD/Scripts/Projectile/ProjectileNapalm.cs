using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNapalm : AProjectile
{
	public delegate void ExplosiveEvent();
	public event ExplosiveEvent Exploded = null;

	[SerializeField]
	private DamageOnZone _napalm;

	[SerializeField]
	private SphereCollider _explosionCollider;

	[SerializeField]
	private float _explosionRadius = 3;

	private float _explosionSpeed = 33;

	private bool _hasExploded = false;



	private void Update()
	{
		MoveForward();
		if (GetHit == true)
		{
			EXPLOSION();
		}
	}

	private void MoveForward()
	{
		if (UseArtilleryMovement)
		{
			if (!_isMoving) StartCoroutine(ArcProjectile());
		}
		else
		{
			transform.position = transform.position + _projectileSpeed * Time.deltaTime * transform.forward;
		}
	}

	private void SpawnNapalm()
	{
		Instantiate(_napalm, transform.position, Quaternion.identity);
	}

	private void EXPLOSION()
	{
		_applyDamage = true;
		_projectileSpeed = 0;
		_explosionCollider.radius = _explosionCollider.radius + _explosionSpeed * Time.deltaTime;

		if (!_hasExploded)
		{
			_hasExploded = true;
			Exploded?.Invoke();
		}

		if (_explosionCollider.radius >= _explosionRadius)
		{
			SpawnNapalm();
			Destroy(gameObject);
		}
	}

	IEnumerator ArcProjectile()
	{
        _isMoving = true;
        float time = 0f;
        Vector3 startPosition = transform.position;

        _destination = Target.transform.position;

        float duration = 60f / _projectileSpeed;

        while (time < duration || !_hitThing)
        {
            time += Time.deltaTime;

            float linearT = time / duration;
            float heightT = _curve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, _projectileMaxHeight, heightT);

            transform.position = Vector3.Lerp(startPosition, _destination, linearT) + new Vector3(0f, height);

            yield return null;
        }

		EXPLOSION();

  //      _isMoving = true;
		//RaycastHit hit;
		//Vector3 destinationPoint = Vector3.zero;
		//if (Physics.Raycast(transform.position, transform.forward, out hit, float.MaxValue, _enemyLayer))
		//{
		//	destinationPoint = hit.point;
		//}

		//float travelDistance = Vector3.Distance(transform.position, destinationPoint);
		//float halfTravelDistance = travelDistance * .5f;
		//float distanceTravelled = 0f;
		//Vector3 currentPos = transform.position;
		//float distanceRatio = 0f;

		//while (distanceTravelled < halfTravelDistance)
		//{
		//	transform.position += (transform.forward * _projectileSpeed * Time.deltaTime);
		//	distanceTravelled += Vector3.Distance(currentPos, transform.position);
		//	currentPos = transform.position;

		//	distanceRatio = 1 - (distanceTravelled / halfTravelDistance);
		//	float rise = _projectileSpeed * distanceRatio * Time.deltaTime;
		//	transform.position += new Vector3(0, rise, 0);

		//	yield return null;
		//}
		//distanceTravelled = 0f;
		//while (distanceTravelled < halfTravelDistance || !_hitThing)
		//{
		//	transform.position += (transform.forward * _projectileSpeed * Time.deltaTime);
		//	distanceTravelled += Vector3.Distance(currentPos, transform.position);
		//	currentPos = transform.position;

		//	distanceRatio = distanceTravelled / halfTravelDistance;
		//	float fall = _projectileSpeed * distanceRatio * Time.deltaTime;
		//	transform.position += new Vector3(0, -fall, 0);

		//	yield return null;
		//}
		//yield return null;
	}
}

