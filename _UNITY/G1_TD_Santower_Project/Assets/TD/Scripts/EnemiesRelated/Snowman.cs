using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
	[SerializeField]
	private float _freezeDuration = 1f;

	[SerializeField]
	private float _speed = 1f;

	[SerializeField]
	private float _distanceThreshold = 0.5f;

	private bool _target = false;

	private GameObject _towerTarget;


	private void Update()
	{
		if (_target == true)
		{
			MoveTo(_towerTarget.transform.position);
			if (Vector3.Distance(transform.position, _towerTarget.transform.position) < _distanceThreshold)
			{
				_towerTarget.GetComponent<Freezer>().Freeze(_freezeDuration);
				
				// Replace with animation script
				Destroy(gameObject);
			}
		}
	}

	private void MoveTo(Vector3 position)
	{
		Vector3 movement = (position - transform.position).normalized * _speed * Time.deltaTime;
		transform.position += movement;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Freezer>() && other.GetComponent<Freezer>().IsFrozen == false)
		{
			_towerTarget = other.gameObject;
			GetComponent<PathFollower>().enabled = false;
			_target = true;
		}
	}
}
