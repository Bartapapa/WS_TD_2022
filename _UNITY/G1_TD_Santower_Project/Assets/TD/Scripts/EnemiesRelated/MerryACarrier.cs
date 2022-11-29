using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MerryACarrier : MonoBehaviour
{
	[SerializeField]
	private Damageable _damageable;

	[SerializeField]
	private float _destinationPositionRandomness = 0;

	[SerializeField]
	private float _moveSpeed = 2f;
	
	[SerializeField]
	private float _rotateSpeed = 2f;

	[SerializeField]
	private float _distanceThreshold = 0.5f;

	[SerializeField]
	private Timer _spawnIntervale;

	[SerializeField]
	private List<WaveEntity> _waveEntity = new List<WaveEntity>();

	[SerializeField]
	private float _randomSpawnAmplitude = 1.0f;

	[SerializeField]
	private Path _merryPath;
	
	private Path _path;

	private GameObject _waypointIndex;

	private List<GameObject> waypoint = new List<GameObject>();

	private int _currentPathIndex = 0;

	private Vector3 _nextDestination;

	private void OnEnable()
	{
		_damageable.CallerDied -= SpawnProcess;
		_damageable.CallerDied += SpawnProcess;
		_spawnIntervale.Start();
	}

	private void OnDisable()
	{
		_damageable.CallerDied -= SpawnProcess;
		_spawnIntervale.Stop();
	}
	private void Update()
	{
		_spawnIntervale.Update();
		if (_spawnIntervale.Progress >= 1)
		{
			SpawnProcess(_damageable, 1, 1);
		}
		UpdateMovement();
	}

	#region Spawn Process

	private void SpawnProcess(Damageable damageable, int currentHealth, int damage)
	{
		GetAllWaypoint();
		GetPath();
		SpawnEnemies();
	}

	private void GetAllWaypoint()
	{
		foreach (GameObject Waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
		{
			waypoint.Add(Waypoint);
		}
	}

	private void GetPath()
	{
		if (waypoint != null)
		{
			var tempGet = waypoint[0];
			for (int i = 0, length = waypoint.Count; i < length; i++)
			{
				float distance = Vector3.Distance(waypoint[i].transform.position, transform.position);
				float targetDistance = Vector3.Distance(tempGet.transform.position, transform.position);

				if (distance < targetDistance)
				{
					tempGet = waypoint[i];
				}
			}
			_waypointIndex = tempGet;
			_path = _waypointIndex.GetComponentInParent<Path>();
		}
	}

	private int GetWaypointIndexInPath()
	{
		int temp = 0;
		for (int i = 0; i < _path.Waypoints.Count; i++)
		{
			if (_path.Waypoints[i].transform.position == _waypointIndex.transform.position)
			{
				temp = i;
			}
		}
		return temp;
	}

	private void SpawnEnemies()
	{
		for (int i = 0; i < _waveEntity.Count; i++)
		{
			RaycastHit hit;
			Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue);
			Vector3 spawnPos = new Vector3(
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.x,
								hit.transform.position.y,
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.z);

			_waveEntity[i].SetPath(_path, false);
			_waveEntity[i].SetWaypoint(GetWaypointIndexInPath());
			Instantiate(_waveEntity[i], spawnPos, Quaternion.identity);
		}
	}

	#endregion Spawn Process

	#region Movement

	private void UpdateMovement()
	{
		if (_merryPath == null)
		{
			return;
		}

		if (Vector3.Distance(transform.position, _nextDestination) < _distanceThreshold && _currentPathIndex < _merryPath.Waypoints.Count)
		{
			_currentPathIndex = _currentPathIndex + 1;
			SetNewDestination(Vector3.zero);
			return;
		}

		if (Vector3.Distance(transform.position, _nextDestination) < _distanceThreshold && _currentPathIndex >= _merryPath.Waypoints.Count)
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
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
	}

	private void SetNewDestination(Vector3 position)
	{
		if (position == Vector3.zero)
		{
			Vector3 randomizedPosition = new Vector3(
								   Random.Range(-_destinationPositionRandomness, _destinationPositionRandomness),
								   0,
								   Random.Range(-_destinationPositionRandomness, _destinationPositionRandomness));

			_nextDestination = _merryPath.Waypoints[_currentPathIndex].position + randomizedPosition;
		}
		else
		{
			_nextDestination = position;
		}


	}

	#endregion Movement
}
