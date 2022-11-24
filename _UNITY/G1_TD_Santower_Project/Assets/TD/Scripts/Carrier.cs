using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
	[SerializeField]
	private List<WaveEntity> _waveEntity= new List<WaveEntity>();

	[SerializeField]
	private float _randomSpawnAmplitude = 1.0f;

	private Path _path;

	private GameObject _waypointIndex;

	private List<GameObject> waypoint = new List<GameObject>();

	private void Start()
	{
		GetPath();
		GetAllWaypoint();
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
			Vector3 spawnPos = new Vector3(
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.x,
								transform.position.y,
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.z);

			_waveEntity[i].SetPath(_path, false);
			_waveEntity[i].SetWaypoint(GetWaypointIndexInPath());
			Instantiate(_waveEntity[i], spawnPos, Quaternion.identity);
		}
	}
}
