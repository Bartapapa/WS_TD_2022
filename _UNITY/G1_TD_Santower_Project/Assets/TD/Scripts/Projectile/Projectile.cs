namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Projectile : AProjectile
	{
        [System.NonSerialized]
        private Quaternion _lastLookRotation = Quaternion.identity;

        private void Update()
		{
			MoveForward();

			if (FollowTarget == true && Target != null)
			{
                LookAt(Target.GetAimPosition());
			}
		}

		private void MoveForward()
		{
                transform.position = transform.position + _projectileSpeed * Time.deltaTime * transform.forward;
        }
        public virtual void LookAt(Vector3 position)
        {
            Vector3 direction = (position - transform.position).normalized;

            _lastLookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, _lastLookRotation, RotationSpeed * Time.deltaTime);

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
}