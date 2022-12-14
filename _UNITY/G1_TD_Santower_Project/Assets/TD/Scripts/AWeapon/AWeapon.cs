namespace GSGD1
{
	using UnityEngine;

	public abstract class AWeapon : MonoBehaviour
	{
        [SerializeField]
        private bool _followTarget = false;

        [SerializeField]
        private float _rotationSpeed = 10;

		[SerializeField]
        private Damageable _target;

        [SerializeField]
		protected Timer _timer = null;

		[SerializeField]
		protected AnimatorHandler_Tower _anim = null;

		public bool FollowTarget { get => _followTarget; set => _followTarget = value; }
        public float RotationSpeed => _rotationSpeed;
        public Damageable Target => _target;


		[SerializeField]
        public DamageableDetector _damageableDetector;

		[SerializeField]
		public bool _removeDamageableCollider = false;

        private void Awake()
		{
            _damageableDetector = GetComponentInParent<DamageableDetector>();
			_anim = GetComponent<AnimatorHandler_Tower>();

            if (_anim == null)
            {
                Debug.Log(name + " doesn't have an animatorHandler. Please advise.");
            }
            else
            {
                _anim.Initialize();
            }
        }

		public virtual bool CanFire()
		{
			return _timer.IsRunning == false;
		}

		protected virtual void Update()
		{
			_timer.Update();
		}
		public virtual void Fire()
		{
			if (CanFire() == true)
			{
				_timer.Start();
				DoFire();
			}
		}

		protected virtual void DoFire()
		{
			if (_anim != null)
			{
                _anim.Animator.SetTrigger("Fire");
            }
		}

		public virtual void AnchorLookAt(Vector3 position)
		{

		}

	}

}