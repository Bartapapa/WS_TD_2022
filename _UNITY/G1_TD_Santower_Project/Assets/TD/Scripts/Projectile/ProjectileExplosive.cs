namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class ProjectileExplosive : AProjectile
	{
		[SerializeField]
		private float _moveSpeed = 1f;

		[SerializeField]
		private SphereCollider _explosionCollider;

		[SerializeField]
		private float _explosionRadius = 3;

		[SerializeField]
		private float _explosionSpeed = 1f;

		private void Update()
		{
			MoveForward();

			if (GetHit == true)
			{
				_moveSpeed = 0;
				_explosionCollider.radius = _explosionCollider.radius + _explosionSpeed * Time.deltaTime;
				if (_explosionCollider.radius >= _explosionRadius)
				{
					Destroy(gameObject);
				}
			}
		}

		private void MoveForward()
		{
			transform.position = transform.position + _moveSpeed * Time.deltaTime * transform.forward;
		}
	}
}