namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class SantaGatling : ProjectileLauncher
	{
		[SerializeField] private float _continueFiringDuration;
		[SerializeField] private bool _isFiring = false;
		[SerializeField] private WeaponController _weaponController;
		[SerializeField] private float _minAngleToFollowTarget;
		private float _continueFiringTimer = 0f;

		public bool IsFiring => _isFiring;

		private void Awake()
		{
			_weaponController = GetComponent<WeaponController>();
			_damageableDetector = GetComponentInParent<DamageableDetector>();
		}

		protected override void Update()
		{
			if (Quaternion.Angle(_weaponController.LastLookRotation, transform.rotation) <= _minAngleToFollowTarget)
			{
				FollowTarget = true;
			}
			else if (Quaternion.Angle(_weaponController.LastLookRotation, transform.rotation) > _minAngleToFollowTarget)
			{
				FollowTarget = false;
			}

			base.Update();
			if (_isFiring)
			{
				if (_continueFiringTimer < _continueFiringDuration)
				{
					_continueFiringTimer += Time.deltaTime;
					Fire();
				}
				else
				{
					_continueFiringTimer = 0f;
					_isFiring = false;
				}
			}

			_anim.Animator.SetBool("isFiring", _isFiring);
		}

		public override void AnchorLookAt(Vector3 position)
		{
			//TODO force anchor to follow WeaponController.forward.

			//Vector3 lookPos = position - transform.position;
			//Quaternion lookRotation = Quaternion.LookRotation(lookPos);
			//_projectileAnchor.rotation = lookRotation;
		}

		public override void Fire()
		{
			base.Fire();
			_isFiring = true;
		}

		public void GatlingUpgrade()
		{
			_timer.Set(0.15f);
		}

		//public override void AnchorLookAt(Vector3 position)
		//{
		//    //TODO force anchor to follow WeaponController.forward.

		//    //Vector3 lookPos = position - transform.position;
		//    //Quaternion lookRotation = Quaternion.LookRotation(lookPos);
		//    //_projectileAnchor.rotation = lookRotation;
		//}

	}
}

