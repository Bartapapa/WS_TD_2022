namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using TMPro;
	using UnityEngine;

	public class ProjectileExplosive : AProjectile
	{
		[SerializeField]
		private float _moveSpeed = 1f;

		[SerializeField]
		private GameObject _explosionRadius;

		private void Start()
		{
			_explosionRadius.SetActive(false);
		}

		private void Update()
		{
			MoveForward();
		}

		private void MoveForward()
		{
			transform.position = transform.position + _moveSpeed * Time.deltaTime * transform.forward;
		}

		public override void OnTriggerEnter(Collider other)
		{
			
		}
	}
}