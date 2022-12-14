namespace GSGD1
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	public class EntitySpawner : MonoBehaviour
	{
		[SerializeField]
		private Transform _instancesRoot = null;

		[SerializeField]
		private Path _path = null;

		[Header("Spawn index")]
		[SerializeField]
		//readonly
		private int _currentSpawnIndex = -1;
		[SerializeField]
		private List<Transform> _phaseWaypointSpawns = new List<Transform>();

		[System.NonSerialized]
		private Timer _timer = new Timer();

		[System.NonSerialized]
		private Wave _wave = null;

		[System.NonSerialized]
		private List<WaveEntity> _runtimeWaveEntities = new List<WaveEntity>();

        [System.NonSerialized]
        private Queue<Wave> _waveQueue = new Queue<Wave>();

        public UnityEvent<EntitySpawner, Wave> WaveStarted = null;
		public UnityEvent<EntitySpawner, Wave> WaveEnded = null;
		public UnityEvent<EntitySpawner, WaveEntity> EntitySpawned = null;

		//public event System.Action<EntitySpawner, WaveEntity> EntityDestroyed = null;

		private void Awake()
		{
			if (_phaseWaypointSpawns.Count < 4)
			{
				Debug.Log(this + "does not have designated phase waypoints for all phases, please fix.");
			}
		}

		private void OnEnable()
		{
			EntitySpawned.RemoveListener(LevelReferences.Instance.SpawnerManager.RegisterEntity);
			EntitySpawned.AddListener(LevelReferences.Instance.SpawnerManager.RegisterEntity);

            GameManager.Instance.GamePhaseChangeEvent_UE.RemoveListener(OnGamePhaseChanged);
            GameManager.Instance.GamePhaseChangeEvent_UE.AddListener(OnGamePhaseChanged);
        }

		private void OnDisable()
		{
            EntitySpawned.RemoveListener(LevelReferences.Instance.SpawnerManager.RegisterEntity);

            GameManager.Instance.GamePhaseChangeEvent_UE.RemoveListener(OnGamePhaseChanged);
        }

		public void StartWave(Wave wave)
		{
			if (_wave != null)
			{
				if (_wave.HasWaveElementsLeft == true)
				{
					_waveQueue.Enqueue(wave);
                    return;
				}
				else
				{
                    _wave = new Wave(wave);
                }
			}
			else
			{
                _wave = new Wave(wave);
                LevelReferences.Instance.SpawnerManager.AddCurrentWaveRunning();
            }
			_timer.Set(wave.DurationBetweenSpawnedEntity).Start();
			WaveStarted?.Invoke(this, wave);
			InstantiateNextWaveElement();
		}

		private WaveEntity InstantiateEntity(WaveEntity entityPrefab)
		{
			WaveEntity entityInstance = Instantiate(entityPrefab, _instancesRoot);
			_runtimeWaveEntities.Add(entityInstance);
			EntitySpawned?.Invoke(this, entityInstance);
			return entityInstance;
		}

		private void InstantiateNextWaveElement()
		{
			if (_wave.HasWaveElementsLeft == true)
			{
                var nextEntity = _wave.GetNextWaveElement();

				if (DatabaseManager.Instance.WaveDatabase.GetWaveElementFromType(nextEntity.EntityType, out WaveEntity outEntity) == true)
				{
					outEntity = InstantiateEntity(outEntity);

                    if (_currentSpawnIndex < 0)
                    {
                        outEntity.SetPath(_path);
                    }
                    else
                    {
                        outEntity.SetPath(_path, false);
						outEntity.SetDestination(_phaseWaypointSpawns[_currentSpawnIndex]);
                        outEntity.transform.position = _phaseWaypointSpawns[_currentSpawnIndex].position;
                    }
					_timer.Set(_wave.DurationBetweenSpawnedEntity + nextEntity.ExtraDurationAfterSpawned).Start();
				}
				else
				{
					Debug.LogErrorFormat("{0}.UpdateWave() cannot GetWaveElementFromType {1}, no corresponding type found in database.", GetType().Name, nextEntity.EntityType);
					return;
				}
			}
			else
			{
				WaveEnded?.Invoke(this, _wave);
				if (_waveQueue.Count >= 1)
				{
                    StartWave(GetNextWave());
                }

			}
		}

		private void Update()
		{
			UpdateWave();
		}

		private void UpdateWave()
		{
			if (_timer != null)
			{
				bool shouldInstantiateEntity = _timer.Update();

				if (shouldInstantiateEntity == true)
				{
					InstantiateNextWaveElement();
				}
			}
		}

		private void OnGamePhaseChanged(GameManager.GamePhase fromPhase, GameManager.GamePhase toPhase)
		{
			int spawnIndex = ((int)toPhase) - 1;
			SetCurrentSpawnIndex(spawnIndex);
        }

		public void SetCurrentSpawnIndex(int index)
		{
			if (index > _phaseWaypointSpawns.Count)
			{
				Debug.Log(name + " Index given is superior to the number of waypoints in phaseWaypointSpawns, ignoring");
				return;
			}

			_currentSpawnIndex = index;
		}
        public Wave PeekNextWave()
        {
            return _waveQueue.Count != 0 ? _waveQueue.Peek() : null;
        }

        public Wave GetNextWave()
        {
            return _waveQueue.Count != 0 ? _waveQueue.Dequeue() : null;
        }
    }
}