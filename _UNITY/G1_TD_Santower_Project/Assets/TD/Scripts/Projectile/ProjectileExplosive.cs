    namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    public class ProjectileExplosive : AProjectile
    {
        [SerializeField]
        private SphereCollider _explosionCollider;

        [SerializeField]
        private float _explosionRadius = 3;

        [SerializeField]
        private float _explosionSpeed = 100;

        private void Update()
        {
            MoveForward();

            if (GetHit == true)
            {
                _projectileSpeed = 0;
                _explosionCollider.radius = _explosionCollider.radius + _explosionSpeed * Time.deltaTime;
                if (_explosionCollider.radius >= _explosionRadius)
                {
                    Destroy(gameObject);
                }
            }
        }

		public float ExplosionRadius { set => _explosionRadius = value; }
		public float ExplosionSpeed { set => _explosionSpeed = value; }

		private void MoveForward()
		{
			if (_useArtilleryMovement)
			{
                if (!_isMoving) StartCoroutine(ArcProjectile());
			}
			else
			{
                transform.position = transform.position + _projectileSpeed * Time.deltaTime * transform.forward;
            }
		}

        IEnumerator ArcProjectile()
        {
            _isMoving = true;
            RaycastHit hit;
            Vector3 destinationPoint = Vector3.zero;
            if (Physics.Raycast(transform.position, transform.forward, out hit, float.MaxValue, _enemyLayer))
            {
                destinationPoint = hit.point;
            }
            
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
            while (distanceTravelled < halfTravelDistance || !_hitThing)
            {
                transform.position += (transform.forward * _projectileSpeed * Time.deltaTime);
                distanceTravelled += Vector3.Distance(currentPos, transform.position);
                currentPos = transform.position;

                distanceRatio = distanceTravelled / halfTravelDistance;
                float fall = _projectileSpeed * distanceRatio * Time.deltaTime;
                transform.position += new Vector3(0, -fall, 0);

                yield return null;
            }
            yield return null;
        }
    }
}