namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ProjectileLauncher : AWeapon
	{
		[SerializeField]
		protected AProjectile _projectile = null;

		[SerializeField]
		protected Transform _projectileAnchor = null;

		[SerializeField]
		protected float _projectileSpeed = 1f;

		[SerializeField]
		protected int _projectileDamage = 2;

		[SerializeField]
		private float _spread = 0;

		protected override void DoFire()
		{
			base.DoFire();

			AProjectile newProjectile = Instantiate(_projectile, _projectileAnchor.position + new Vector3(Random.Range(-_spread, _spread), Random.Range(-_spread, _spread), Random.Range(-_spread, _spread)), _projectileAnchor.rotation);
			newProjectile.SetProjectileSPeed(_projectileSpeed);
			newProjectile.SetDamage(_projectileDamage);
		}

        public override void AnchorLookAt(Vector3 position)
        {
			//TODO force anchor to follow WeaponController.forward.

			Vector3 lookPos = position - transform.position;
			Quaternion lookRotation = Quaternion.LookRotation(lookPos);
			_projectileAnchor.rotation = lookRotation;
		}
    }
}