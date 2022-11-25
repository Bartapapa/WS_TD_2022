namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using Unity.Collections;
	using UnityEngine;

	public class PathFollower : MonoBehaviour
	{
		[SerializeField]
		private Path _path = null;

		[SerializeField]
		private float _moveSpeed = 1f;

		[SerializeField]
		private float _rotateSpeed = 1f;

		[SerializeField]
		private float _maxRandoMultiplier = 1.1f;

		[SerializeField]
		private float _minRandoMultiplier = 0.9f;

		[SerializeField]
		private float _distanceThreshold = 0.5f;

		private int _currentPathIndex = 0;
		private int _waypointIndex = 0;
	
		private bool _grosBool = false;

		private void Start()
		{
			_moveSpeed = _moveSpeed * Random.Range(_minRandoMultiplier, _maxRandoMultiplier);
		}

		public void SetWaypoint(int indexPath)
		{
			_waypointIndex = indexPath;
			_currentPathIndex = indexPath;
		}

		public void SetCanMove(bool canMove)
		{
			this.enabled = canMove;
		}

		public void SetPath(Path path, bool teleportToFirstWaypoint = true)
		{
			_path = path;
			if (teleportToFirstWaypoint == true)
			{
				Transform firstWaypoint = _path.FirstWaypoint;
				if (firstWaypoint != null)
				{
					transform.position = firstWaypoint.position;
				}
			}
		}

		private void Update()
		{
			if (_path == null || _currentPathIndex >= _path.Waypoints.Count)
			{
				return;
			}
			if (_grosBool == false)
			{
				SetWaypoint(_waypointIndex);
				_grosBool = true;
			}
			Vector3 nextDestination = _path.Waypoints[_currentPathIndex].position;

			if (Vector3.Distance(transform.position, nextDestination) < _distanceThreshold)
			{
				_currentPathIndex = _currentPathIndex + 1;
				return;
			}

			MoveTo(nextDestination);
			LookAt(nextDestination);
		}

		private void MoveTo(Vector3 position)
		{
			Vector3 movement = (position - transform.position).normalized * _moveSpeed * Time.deltaTime;
			transform.position += movement;
		}

		private void LookAt(Vector3 position)
		{
			//transform.LookAt(position, Vector3.up);

			Vector3 direction = position - transform.position;
			direction.y = 0;
			Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
			transform.rotation =  Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
		}
	}
}