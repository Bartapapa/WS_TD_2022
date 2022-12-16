using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrier : MonoBehaviour
{
	[SerializeField]
	private Damageable _damageable;

	[SerializeField]
	private bool _isMerryACarrier = false;

	[SerializeField]
	private Timer _spawnIntervale;

	[SerializeField]
	private float _randomSpawnAmplitude = 1.0f;
	
	[SerializeField]
	private List<WaveEntityGroupDescription> _waveEntity = new List<WaveEntityGroupDescription>();

	[SerializeField]
	private AnimatorHandler_Entity _anim;

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
	}

	private void OnDisable()
	{
		_damageable.CallerDied -= SpawnProcess;
	}

	private void Awake()
	{ 
		_waveEntityDatas = DatabaseManager.Instance.WaveDatabase;

		_anim = GetComponent<AnimatorHandler_Entity>();
	}

	private void Update()
	{
		if (_isMerryACarrier == true)
		{
			if (_found == false)
			{
				_pathFollower = GetComponent<PathFollower>();
				GetAllWaypoint(true);
				GetPath();
				_pathFollower.SetPath(_merryPath, false);
			}

			_spawnIntervale.Update();
			if (_spawnIntervale.Progress >= 1)
			{
				_spawnIntervale.Update();
				SpawnProcess(_damageable, 1, 1);

				if (_isMerryACarrier)
				{
					_anim.Animator.SetTrigger("Spawn");
				}
			}
		}
	}
	private void SpawnProcess(Damageable damageable, int currentHealth, int damage)
	{
		if (currentHealth <= 0)
		{
			_dead = true;
		}

		GetAllWaypoint(false);
		GetPath();
		SpawnEnemies();
	}

	private void GetAllWaypoint(bool findMerryPath = false)
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
		var moshIndex = 0;
		if (_isMerryACarrier == true)
		{
			if (_dead == true)
			{
				moshIndex = 0;
			}
			else
			{
				moshIndex = Random.Range(1, _waveEntity.Count);
			}
		}

		for (int y = 0; y < _waveEntity[moshIndex].GetWaves.Count; y++)
		{
			for (int i = 0; i < _waveEntity[moshIndex].GetWaves[y].WaveEntitiesDescription.Count; i++)
			{
				_waveEntityDatas.GetWaveElementFromType(_waveEntity[moshIndex].GetWaves[y].WaveEntitiesDescription[i].EntityType, out _entity);
				RaycastHit hit;
				Physics.Raycast(transform.position, Vector3.down, out hit, float.MaxValue);
				Vector3 spawnPos = new Vector3(
									Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.x,
									hit.transform.position.y,
									Random.Range(-_randomSpawnAmplitude, _randomSpawnAmplitude) + transform.position.z);

				_entity.SetPath(_path, false);
				_entity.SetWaypoint(GetWaypointIndexInPath());
				_entity.DisableDropOnDeath();
				Instantiate(_entity, spawnPos, Quaternion.identity);
			}
		}
	}
}
