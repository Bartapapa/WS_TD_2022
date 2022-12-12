namespace GSGD1
{
    using GSGD1;
    using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class WaveEntity : MonoBehaviour
	{
        [SerializeField]
		private PathFollower _pathFollower = null;

        [SerializeField]
		private Damageable _damageable = null;

        [SerializeField]
        private AnimatorHandler_Entity _anim;

        private Transform _northPoleTarget;


        [Header("Animation Handler")]
        [SerializeField]
        private int _forceWalkValue = -1;
        [SerializeField]
        private int _forceDeathValue = -1;
        public AnimatorHandler_Entity AnimatorHandler => _anim;

        private bool _isAir = false;
        private bool _isFallingToDeath = false;

		private void Awake()
		{
            _pathFollower = GetComponent<PathFollower>();
            _damageable = GetComponent<Damageable>();
            _northPoleTarget = LevelReferences.Instance.NorthPole.transform;

            _isAir = _damageable.GetIsFlying;

            if (_anim == null)
            {
                Debug.Log(name + " doesn't have an animatorHandler. Please advise.");
            }
            else
            {
                _anim.SetWalkAnimation(_forceWalkValue);
            }
		}

        private void OnEnable()
        {
            _damageable.DamageTaken -= OnDamageTaken;
            _damageable.DamageTaken += OnDamageTaken;

            _damageable.CallerDied -= OnCallerDied;
            _damageable.CallerDied += OnCallerDied;
        }

        private void OnDisable()
        {
            _damageable.DamageTaken -= OnDamageTaken;

            _damageable.CallerDied -= OnCallerDied;
        }

        public void SetPath(Path path, bool teleportToFirstWaypoint = true)
		{
            if (_pathFollower != null)
            {
			    _pathFollower.SetPath(path, teleportToFirstWaypoint);
            }
		}

        private void Update()
        {
            if (Vector3.Distance(transform.position, _northPoleTarget.position) <= 5)
            {
                if (_anim != null)
                {
                    _anim.Animator.SetTrigger("Attack");
                }

            }

            if (_isFallingToDeath)
            {
                transform.position += (Vector3.down * _pathFollower.Speed * Time.deltaTime) + (transform.forward * _pathFollower.Speed * Time.deltaTime);
            }
        }

        public void SetWaypoint(int waypointIndex)
		{
            if (_pathFollower != null)
            {
			    _pathFollower.SetWaypoint(waypointIndex);
            }
		}

        public void SetDestination (Transform destination)
        {
            _pathFollower.SetWaypointDestination(destination);
        }

        public void DisableDropOnDeath()
        {
            _pathFollower.DisableDropOnDeath();
        }

        public IEnumerator HitFlash(float duration, Color color, float intensityFlash)
        {
            float flashDuration = duration;
            float flashIntensity = intensityFlash;
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            while (flashDuration >= 0f)
            {
                flashDuration -= Time.deltaTime;
                float lerp = Mathf.Clamp01(flashDuration / duration);
                float intensity = lerp * flashIntensity + 1;
                foreach (Renderer renderer in renderers)
                {
                    for (int x = 0; x < renderer.materials.Length; x++)
                    {
                        renderer.materials[x].color = color * intensity;
                    }
                }
                yield return null;
            }
            foreach (Renderer renderer in renderers)
            {
                for (int x = 0; x < renderer.materials.Length; x++)
                {
                    renderer.materials[x].color = Color.white * 1f;
                }
            }
        }

        void OnDamageTaken(Damageable caller, int currentHealth, int damageTaken)
        {
            if (damageTaken != 0)
            {
                StartCoroutine(HitFlash(.1f, Color.white, 500f));
            }

            //particles, etc
        }

        void OnCallerDied(Damageable caller, int currentHealth, int damageTaken)
        {
            _pathFollower.SetCanMove(false);

            if (_anim != null)
            {
                _anim.SetDeathAnimation(_forceDeathValue);
                _anim.Animator.SetTrigger("Die");
            }


            if (_isAir)
            {
                _isFallingToDeath = true;
            }
        }
    }
}