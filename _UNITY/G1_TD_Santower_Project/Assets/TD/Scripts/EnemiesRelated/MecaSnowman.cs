using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecaSnowman : MonoBehaviour
{
	[SerializeField]
	private AnimatorHandler _anim;

	[SerializeField]
	private Damageable _damageable;

    [SerializeField]
    private float _fireRadius = 10f;

    [SerializeField]
    private WeaponController _weaponController;

    [SerializeField]
	private Timer _spawnIntervale;

	[SerializeField]
	private WaveEntityGroupDescription _waveEntities;

	[SerializeField]
	private List<EntitySpawner> _spawner = new List<EntitySpawner>();

	private WaveDatabase _waveEntityDatas;

	private List<GameObject> _tower = new List<GameObject>();

	private List<GameObject> _waypoint = new List<GameObject>();

	private Path _path;

	private GameObject _waypointIndex;

	private WaveEntity _entity;

	private PathFollower _pathFollower;

    private void Awake()
    {
		_waveEntityDatas = DatabaseManager.Instance.WaveDatabase;

		_pathFollower = GetComponent<PathFollower>();
		_anim = GetComponent<WaveEntity>().AnimatorHandler;
		
		foreach (EntitySpawner spawner in LevelReferences.Instance.SpawnerManager.Spawner)
		{
			if (spawner.tag != "Air")
			{
				_spawner.Add(spawner);
			}
		}
	}

    private void OnEnable()
	{
		_damageable.CallerDied -= OnDeath;
		_damageable.CallerDied += OnDeath;
		_spawnIntervale.Start();
	}

	private void OnDisable()
	{
		_damageable.CallerDied -= OnDeath;
		_spawnIntervale.Stop();
	}

	private void Update()
	{
		_spawnIntervale.Update();
		if (_spawnIntervale.Progress >= 1)
		{
			//TODO make the hordeSpawn an event called in animation.
			_anim.Animator.SetTrigger("Call");
			HordeSpawn();
		}
	}

    private void OnDeath(Damageable damageable, int currentHealth, int damageTaken)
	{
		foreach (GameObject tower in _tower)
		{
			tower.GetComponent<Freezer>().Unfreeze();
		}
	}

	private void HordeSpawn()
	{
		foreach (EntitySpawner spawner in _spawner)
		{
			GetPath(spawner);
			SpawnEnemies(spawner);
		}
	}

	private void GetAllWaypoint()
	{
		foreach (GameObject waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
		{
			_waypoint.Add(waypoint);
		}
	}

	private void GetPath(EntitySpawner spawner)
	{
		GetAllWaypoint();
		var tempGet = _waypoint[0];
		for (int i = 0, length = _waypoint.Count; i < length; i++)
		{
			float distance = Vector3.Distance(_waypoint[i].transform.position, spawner.transform.position);
			float targetDistance = Vector3.Distance(tempGet.transform.position, spawner.transform.position);

			if (distance < targetDistance)
			{
				tempGet = _waypoint[i];
			}
		}
		_waypointIndex = tempGet;
		_path = _waypointIndex.GetComponentInParent<Path>();
		Debug.Log(_path);
	}

	private void SpawnEnemies(EntitySpawner spawner)
	{
		for (int y = 0; y < _waveEntities.GetWaves.Count; y++)
		{
			for (int i = 0; i < _waveEntities.GetWaves[y].WaveEntitiesDescription.Count; i++)
			{
				_waveEntityDatas.GetWaveElementFromType(_waveEntities.GetWaves[y].WaveEntitiesDescription[i].EntityType, out _entity);
				Vector3 spawnPos = spawner.transform.position;

				_entity.SetPath(_path, false);
				Instantiate(_entity, spawnPos, Quaternion.identity);
			}
		}
	}

	private void LookAt(Vector3 position)
	{
		Vector3 direction = position - transform.position;
		direction.y = 0;
		Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10 * Time.deltaTime);
	}

	private void Freezing(Collider other)
	{
		_pathFollower.SetCanMove(false);
		LookAt(other.transform.position);
		_anim.Animator.SetTrigger("Freeze");
		_weaponController.LookAtAndFire(other.transform.position);
		_tower.Add(other.gameObject);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponentInParent<Freezer>().IsFrozen == false)
		{
			Freezing(other);

			_pathFollower.SetCanMove(true);
		}
	}
}
