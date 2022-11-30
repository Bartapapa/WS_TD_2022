using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
	[SerializeField]
	private Damageable _damageable;

	[SerializeField]
	private List<WaveEntity> _waveEntity= new List<WaveEntity>();

	[SerializeField]
	private float _randomSpawnAmplitude = 1.0f;

	private Path _path;

	private GameObject _waypointIndex;

	private List<GameObject> waypoint = new List<GameObject>();

	private void OnEnable()
	{
		_damageable.CallerDied -= SpawnProcess;
		_damageable.CallerDied += SpawnProcess;
	}

	private void OnDisable()
	{
		_damageable.CallerDied -= SpawnProcess;
	}

	private void SpawnProcess(Damageable damageable, int currentHealth, int damage)
	{
		GetAllWaypoint(false);
		GetPath();
		SpawnEnemies();
	}

	private void GetAllWaypoint(bool findMerryPath)
	{
		waypoint.Clear();
		foreach (GameObject Waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
		{
			if (Waypoint.GetComponentInParent<MerryPath>() == false)
			{
				waypoint.Add(Waypoint);
			}
			else if (findMerryPath == true)
			{
				waypoint.Add(Waypoint);
			}
		}
	}

	private void GetPath()
	{
		if (waypoint != null)
		{
			var tempGet = waypoint[0];
			for (int i = 0, length = waypoint.Count; i < length; i++)
			{
                RaycastHit hit;
                Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue);

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
		int index = 0;
		for (int i = 0; i < _path.Waypoints.Count; i++)
		{
			if (_path.Waypoints[i].transform.position == _waypointIndex.transform.position)
			{
				index = i;
			}
		}
		return index;
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
}
