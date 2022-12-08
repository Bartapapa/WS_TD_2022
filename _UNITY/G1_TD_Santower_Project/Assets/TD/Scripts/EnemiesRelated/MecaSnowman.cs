using GSGD1;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecaSnowman : MonoBehaviour
{
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

	private List<GameObject> waypoint = new List<GameObject>();

	private Path _path;

	private GameObject _waypointIndex;

	private WaveEntity _entity;

	private PathFollower _pathFollower;

    private void Awake()
    {
		_waveEntityDatas = DatabaseManager.Instance.WaveDatabase;

		_pathFollower = GetComponent<PathFollower>();
        _pathFollower.SetPath(_path, true);

        GetAllTurret();
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
			HordeSpawn();
		}
       
		//var tower = GetNearestTower();
  //      if (tower != null)
  //      {
  //          if (Vector3.Distance(tower.transform.position, transform.position) <= _fireRadius && tower.GetComponent<Freezer>().IsFrozen == false)
		//	{ 
		//		_tower.Add(tower);

  //              _pathFollower.SetCanMove(false);
  //              _weaponController.LookAtAndFire(tower.transform.position);
  //          }
  //          else
  //          {
  //              _pathFollower.SetCanMove(true);
  //          }
  //      }
  //      RemoveNullItemsFromList();
    }

    private void GetAllTurret()
    {
        foreach (GameObject Tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            _tower.Add(Tower);
        }
    }

    public void RemoveNullItemsFromList()
    {
        for (var i = _tower.Count - 1; i > -1; i--)
        {
            if (_tower[i] == null)
            {
                _tower.RemoveAt(i);
            }
        }
    }

    private GameObject GetNearestTower()
    {
        float shortestDistance = 0;
        int shortestDistanceIndex = 0;
        for (int i = 0; i < _tower.Count; i++)
        {
            if (_tower[i] != null)
            {
                var distance = (_tower[i].transform.position - transform.position).sqrMagnitude;
                if (distance < shortestDistance && _tower[i].GetComponent<Freezer>().IsFrozen == false)
                {
                    shortestDistance = distance;
                    shortestDistanceIndex = i;
                }
            }
        }
        return _tower[shortestDistanceIndex];
    }

    private void OnDeath(Damageable damageable, int currentHealth, int damageTaken)
	{
		foreach (GameObject Tower in _tower)
		{
			Tower.GetComponent<Freezer>().Unfreeze();
		}
	}

	private void HordeSpawn()
	{
		foreach (EntitySpawner Spawner in _spawner)
		{
			GetPath(Spawner);
			SpawnEnemies(Spawner);
		}
	}

	private void GetAllWaypoint()
	{
		foreach (GameObject Waypoint in GameObject.FindGameObjectsWithTag("Waypoint"))
		{
			waypoint.Add(Waypoint);
		}
	}

	private void GetPath(EntitySpawner spawner)
	{
		GetAllWaypoint();
		var tempGet = waypoint[0];
		for (int i = 0, length = waypoint.Count; i < length; i++)
		{
			float distance = Vector3.Distance(waypoint[i].transform.position, spawner.transform.position);
			float targetDistance = Vector3.Distance(tempGet.transform.position, spawner.transform.position);

			if (distance < targetDistance)
			{
				tempGet = waypoint[i];
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Freezer>() && other.GetComponent<Freezer>().IsFrozen == false)
		{
			other.GetComponent<Freezer>().Freeze(99999);
			_tower.Add(other.gameObject);
		}
	}
}
