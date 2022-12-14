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
		private DropOnDeath _dropOnDeath;

		[SerializeField]
		private bool _pathLoop = false;

		[SerializeField]
		private float _moveSpeed = 1f;

		[SerializeField]
		private float _rotateSpeed = 1f;

		[SerializeField]
		private float _maxSpeedRandoMultiplier = 1.1f;

		[SerializeField]
		private float _minSpeedRandoMultiplier = 0.9f;

		[SerializeField]
		private float _distanceThreshold = 0.5f;

		[SerializeField]
		private float _destinationPositionRandomness = 2.5f;
		
		[SerializeField]
		private int _currentPathIndex;

		public float Speed => _moveSpeed;

		private bool _grosBool = false;

		private Vector3 _nextDestination;

		private void Start()
		{
			_moveSpeed = _moveSpeed * Random.Range(_minSpeedRandoMultiplier, _maxSpeedRandoMultiplier);
		}

		public void SetWaypoint(int indexPath)
		{
			_currentPathIndex = indexPath;

            SetNewDestination(Vector3.zero);
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

		public void DisableDropOnDeath()
		{
			_dropOnDeath.enabled= false;
		}

		private void Update()
		{
			if (_path == null || _currentPathIndex >= _path.Waypoints.Count)
			{
				return;
			}
			if (_grosBool == false)
			{
				SetWaypoint(_currentPathIndex);
				_grosBool = true;
			}

			if (Vector3.Distance(transform.position, _nextDestination) < _distanceThreshold && _currentPathIndex < _path.Waypoints.Count - 1)
			{
				_currentPathIndex = _currentPathIndex + 1;
				SetNewDestination(Vector3.zero);
				return;
			}
			else if (Vector3.Distance(transform.position, _nextDestination) < _distanceThreshold && _currentPathIndex >= _path.Waypoints.Count - 1 && _pathLoop == true)
			{
				_currentPathIndex = 0;
				SetNewDestination(Vector3.zero);
				return;
			}

			MoveTo(_nextDestination);
			LookAt(_nextDestination);
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

		private void SetNewDestination(Vector3 position)
		{
			if (position == Vector3.zero)
			{
                Vector3 randomizedPosition = Vector3.zero;
                if (_currentPathIndex != 0 && _currentPathIndex != _path.Waypoints.Count - 1 && _pathLoop == false)
                {
                    randomizedPosition = new Vector3(
											Random.Range(-_destinationPositionRandomness, _destinationPositionRandomness),
                                            0,
                                            Random.Range(-_destinationPositionRandomness, _destinationPositionRandomness));
                }

                _nextDestination = _path.Waypoints[_currentPathIndex].position + randomizedPosition;
            }
			else
			{
				_nextDestination = position;
			}
		}

		public void SetWaypointDestination(Transform waypoint)
		{
			int indexDestination = -1;

			for (int i = 0; i < _path.Waypoints.Count; i++)
			{
				if (waypoint == _path.Waypoints[i])
				{
					indexDestination = i;
				}
			}

			if (indexDestination < 0)
			{
				_currentPathIndex = 0;
                SetNewDestination(Vector3.zero);
			}
			else
			{
				_currentPathIndex = indexDestination;
				SetNewDestination(Vector3.zero);
			}
		}
	}
}