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
    private Timer _spawnIntervale;

    [SerializeField]
    private WaveEntityGroupDescription _waveEntities;

    [SerializeField]
    private List<EntitySpawner> _spawner = new List<EntitySpawner>();

    private WaveDatabase _waveEntityDatas;

    private List<GameObject> _waypoint = new List<GameObject>();

    private Path _path;

    private GameObject _waypointIndex;

    private WaveEntity _entity;

    private void Awake()
    {
        _waveEntityDatas = DatabaseManager.Instance.WaveDatabase;

        _anim = GetComponentInParent<WaveEntity>().AnimatorHandler;

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
        _spawnIntervale.Start();
    }

    private void OnDisable()
    {
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
}
