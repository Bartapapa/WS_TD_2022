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
	private float _randomSpawnAmplitude = 1.0f;

	[SerializeField]
	private Timer _spawnIntervale;

	[SerializeField]
	private List<WaveEntity> _waveEntity = new List<WaveEntity>();

	private PathFollower _pathFollower;

	private Path _path;

	private Path _merryPath;

	private GameObject _waypointIndex;

	private List<GameObject> waypoint = new List<GameObject>();

	private bool _found = false;

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
		if (_found == false)
		{
			_pathFollower = GetComponent<PathFollower>();
			GetAllWaypoint();
			GetPath();
			_pathFollower.SetPath(_merryPath, false);
		}

		_spawnIntervale.Update();
		if (_spawnIntervale.Progress >= 1)
		{
			SpawnProcess(_damageable, 1, 1);
		}
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
				if (waypoint[i].GetComponentInParent<MerryPath>() == true && _found == false)
				{
					_merryPath = waypoint[i].GetComponentInParent<Path>();
					_found = true;
				}

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

}
