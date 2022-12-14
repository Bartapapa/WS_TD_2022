    namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

    public class ProjectileExplosive : AProjectile
    {
        public delegate void ExplosiveEvent();
        public event ExplosiveEvent Exploded = null;

        [SerializeField]
        private SphereCollider _explosionCollider;

        [SerializeField]
        private bool _impactNeeded = true;

        [SerializeField]
        private float _explosionRadius = 3;

        private float _explosionSpeed = 33;

        [SerializeField]
        private bool _accelerateToMaxSpeed = false;
        [SerializeField]
        private float _acceleration = 1f;
        private float _currentSpeed = 0f;

        [SerializeField]
        private Lifespan _lifespan;

        [SerializeField]
        private bool _canProduceShrapnel = false;
        private bool _hasExploded = false;

        public void SetDestination(Vector3 setDestination)
        {
            _destination = setDestination;
        }
        public bool CanProduceShrapnel => _canProduceShrapnel;

        private void Awake()
        {
            _lifespan = GetComponent<Lifespan>();
        }

        private void Start()
        {
            ExplosionSpeed = _explosionSpeed * _explosionRadius;
        }

        private void Update()
        {
            MoveForward();
            if (_impactNeeded == false)
            {
                EXPLOSION();
            }

            if (GetHit == true)
            {
                EXPLOSION();
            }

            if (_lifespan.LifeSpanTimer.Progress >= .9)
            {
                EXPLOSION();
            }
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

		public float ExplosionRadius { set => _explosionRadius = value; }
		public float ExplosionSpeed { set => _explosionSpeed = value; }

		private void MoveForward()
		{
			if (UseArtilleryMovement)
			{
                if (!_isMoving) StartCoroutine(ArcProjectile());
			}
			else
			{
                if (_accelerateToMaxSpeed)
                {
                    float newSpeed = Mathf.Lerp(_currentSpeed, _projectileSpeed, _acceleration * Time.deltaTime);
                    _currentSpeed = newSpeed;
                    transform.position = transform.position + _currentSpeed * Time.deltaTime * transform.forward;
                }
                else
                {
                    transform.position = transform.position + _projectileSpeed * Time.deltaTime * transform.forward;
                }
            }
		}

        IEnumerator ArcProjectile()
        {
            _isMoving = true;
            float time = 0f;
            Vector3 startPosition = transform.position;

            _destination = Target.transform.position - Target.transform.forward;

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

            Debug.Log("BOOM!");

            EXPLOSION();

            //_isMoving = true;
            //RaycastHit hit;
            //Vector3 destinationPoint = Vector3.zero;
            //if (Physics.Raycast(transform.position, transform.forward, out hit, float.MaxValue, _enemyLayer))
            //{
            //    destinationPoint = hit.point;
            //}
            
            //float travelDistance = Vector3.Distance(transform.position, destinationPoint);
            //float halfTravelDistance = travelDistance * .5f;
            //float distanceTravelled = 0f;
            //Vector3 currentPos = transform.position;
            //float distanceRatio = 0f;

            //while (distanceTravelled < halfTravelDistance)
            //{
            //    transform.position += (transform.forward * _projectileSpeed * Time.deltaTime);
            //    distanceTravelled += Vector3.Distance(currentPos, transform.position);
            //    currentPos = transform.position;

            //    distanceRatio = 1 - (distanceTravelled / halfTravelDistance);
            //    float rise = _projectileSpeed * distanceRatio * Time.deltaTime;
            //    transform.position += new Vector3(0, rise, 0);

            //    yield return null;
            //}
            //distanceTravelled = 0f;
            //while (distanceTravelled < halfTravelDistance || !_hitThing)
            //{
            //    transform.position += (transform.forward * _projectileSpeed * Time.deltaTime);
            //    distanceTravelled += Vector3.Distance(currentPos, transform.position);
            //    currentPos = transform.position;

            //    distanceRatio = distanceTravelled / halfTravelDistance;
            //    float fall = _projectileSpeed * distanceRatio * Time.deltaTime;
            //    transform.position += new Vector3(0, -fall, 0);

            //    yield return null;
            //}
            //yield return null;
        }

        public void SetCanProduceShrapnel(bool value)
        {
            _canProduceShrapnel = value;
        }
    }
}