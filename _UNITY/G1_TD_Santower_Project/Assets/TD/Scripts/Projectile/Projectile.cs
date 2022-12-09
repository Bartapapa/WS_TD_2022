namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class Projectile : AProjectile
	{
        private void Update()
		{
			MoveForward();
		}

		private void MoveForward()
		{
                transform.position = transform.position + _projectileSpeed * Time.deltaTime * transform.forward;
        }
	}
}