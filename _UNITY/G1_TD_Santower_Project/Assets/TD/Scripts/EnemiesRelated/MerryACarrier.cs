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
	private List<WaveEntityGroupDescription> _waveEntity = new List<WaveEntityGroupDescription>();

	[SerializeField]
	private WaveDatabase _waveEntityDatas;

	private PathFollower _pathFollower;

	private Path _path;

	private Path _merryPath;

	private GameObject _waypointIndex;

	private List<GameObject> waypoint = new List<GameObject>();

	private WaveEntity _entity;

	private bool _found = false;

	private bool _dead = false;

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
		//if (currentHealth <= 0)
		//{
		//	_dead = true;
		//}

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

				RaycastHit hit;
				Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue);

				float distance = Vector3.Distance(waypoint[i].transform.position, hit.transform.position);
				float targetDistance = Vector3.Distance(tempGet.transform.position, hit.transform.position);

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
		var moshIndex = 0;
		if (_dead == true)
		{
			moshIndex = 0;
		}
		else
		{
			moshIndex = Random.Range(1, _waveEntity.Count);
		}

		for (int y = 0; y < _waveEntity[moshIndex].GetWaves[y].WaveEntitiesDescription.Count; y++)
		{
			_waveEntityDatas.GetWaveElementFromType(_waveEntity[moshIndex].GetWaves[y].WaveEntitiesDescription[y].EntityType, out _entity);
			RaycastHit hit;
			Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue);
			Vector3 spawnPos = new Vector3(
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.x,
								hit.transform.position.y,
								Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.z);

			_entity.SetPath(_path, false);
			_entity.SetWaypoint(GetWaypointIndexInPath());
			Instantiate(_entity, spawnPos, Quaternion.identity);
		}
	}

	#endregion Spawn Process

}
