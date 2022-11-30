namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;

	public enum SpawnerIndex
	{
		Spawner00,
		Spawner01,
		Spawner02,
	}

	public enum SpawnerStatus
	{
		Inactive = 0,
		WaveRunning
	}

	public class SpawnerManager : MonoBehaviour
	{
		[SerializeField]
		private List<EntitySpawner> _spawners = null;

		[SerializeField]
		private bool _autoStartNextWaves = false;

		[System.NonSerialized]
		private int _currentWaveSetIndex = -1;

		[System.NonSerialized]
		private int _currentWaveRunning = 0;

		[SerializeField]
		public UnityEvent<SpawnerManager, SpawnerStatus, int> WaveStatusChanged_UnityEvent = null;
        public UnityEvent WaveStatusStartedSpawning = null;
        public UnityEvent WaveStatusFinishedSpawning = null;
        public UnityEvent WaveStatusEnded = null;

        [System.NonSerialized]
		private Coroutine _waitForNextWaveCoroutine;

		public delegate void SpawnerEvent(SpawnerManager sender, SpawnerStatus status, int runningWaveCount);
		public event SpawnerEvent WaveStatusChanged = null;

        [SerializeField]
        private int _enemiesKilled = 0;
        [SerializeField]
        private int _enemiesBombed = 0;
        [System.NonSerialized]
        private int _numberOfWaveSets;
        [System.NonSerialized]
        private int[] _numberOfEnemiesPerWave;

        [SerializeField]
        private List<Damageable> _livingEntities = new List<Damageable>();

        [ContextMenu("Start waves")]
		public void StartWaves()
		{
			// Start a new wave set only if there are no currently a wave running
			if (_currentWaveRunning <= 0)
			{
				StartNewWaveSet();
			}
		}

		private void Awake()
		{
            _numberOfWaveSets = DatabaseManager.Instance.WaveDatabase.Waves.Count;
            _numberOfEnemiesPerWave = new int[_numberOfWaveSets];

            for (int i = 0; i < _numberOfWaveSets; i++)
            {
                _numberOfEnemiesPerWave[i] = 0;

				for (int x = 0; x < DatabaseManager.Instance.WaveDatabase.Waves[i].Waves.Count; x++)
				{
					_numberOfEnemiesPerWave[i] += DatabaseManager.Instance.WaveDatabase.Waves[i].Waves[x].WaveEntitiesDescription.Count;
				}
            }

            for (int i = 0; i < _numberOfEnemiesPerWave.Length; i++)
            {
                Debug.Log("WaveSet " + i + " has " + _numberOfEnemiesPerWave[i] + " entities inside.");
            }
        }

		public void StartNewWaveSet()
		{
			_currentWaveSetIndex += 1;
            _enemiesKilled = 0;
            _enemiesBombed = 0;
            var waveDatabase = DatabaseManager.Instance.WaveDatabase;

			if (waveDatabase.Waves.Count > _currentWaveSetIndex)
			{
				WaveSet waveSet = waveDatabase.Waves[_currentWaveSetIndex];
				List<Wave> waves = new List<Wave>();
				foreach (WaveEntityGroupDescription WEGDef in waveSet.Waves)
				{
					foreach (Wave wave in WEGDef.GetWaves)
					{
						waves.Add(wave);
					}
				}

				for (int i = 0, length = _spawners.Count; i < length; i++)
				{
					if (i >= waves.Count)
					{
						Debug.LogWarningFormat("{0}.StartNewWaveSet() There are more spawner ({1}) than wave ({2}), discarding wave.", GetType().Name, _spawners.Count, waves.Count);
						break;
					}
					if (waves[i] == null)
					{
						Debug.LogWarningFormat("{0}.StartNewWaveSet() Null reference found in WaveSet at index {1}, ignoring.", GetType().Name, i);
						break;
					}
					_currentWaveRunning += 1;
					var spawner = _spawners[i];
					spawner.StartWave(waves[i]);
					spawner.WaveEnded.RemoveListener(Spawner_OnWaveEnded);
					spawner.WaveEnded.AddListener(Spawner_OnWaveEnded);

					WaveStatusChanged?.Invoke(this, SpawnerStatus.WaveRunning, _currentWaveRunning);
					WaveStatusChanged_UnityEvent?.Invoke(this, SpawnerStatus.WaveRunning, _currentWaveRunning);
				}
			}
			else
			{
                WaveStatusEnded.Invoke();
                Debug.Log("No waves left!");
                // No waves left : end game
            }
		}

		private void Spawner_OnWaveEnded(EntitySpawner entitySpawner, Wave wave)
		{
			entitySpawner.WaveEnded.RemoveListener(Spawner_OnWaveEnded);

			_currentWaveRunning -= 1;

			//WaveStatusChanged?.Invoke(this, SpawnerStatus.Inactive, _currentWaveRunning);
			//WaveStatusChanged_UnityEvent?.Invoke(this, SpawnerStatus.Inactive, _currentWaveRunning);

			// should we run a new wave?
			if (_autoStartNextWaves == true && _currentWaveRunning <= 0)
			{
				// prevent overlapping routines
				if (_waitForNextWaveCoroutine != null)
				{
					StopCoroutine(_waitForNextWaveCoroutine);
				}
				_waitForNextWaveCoroutine = StartCoroutine(WaitForNewWaveSet());
			}
		}

		private IEnumerator WaitForNewWaveSet()
		{
			var waveDatabase = DatabaseManager.Instance.WaveDatabase;
			float waitingDuration = waveDatabase.Waves[_currentWaveSetIndex].WaitingDurationBefore;
			
			if (_currentWaveSetIndex - 1 > 0)
			{
				waitingDuration += waveDatabase.Waves[_currentWaveSetIndex - 1].WaitingDurationAfter;
			}

			Debug.LogFormat("Waiting {0} seconds until next wave.", waitingDuration);
			yield return new WaitForSeconds(waitingDuration);

			_waitForNextWaveCoroutine = null;
			StartNewWaveSet();
		}

        public void RegisterEntity(EntitySpawner entitySpawner, WaveEntity waveEntity)
        {
            //This function is linked to the EntitySpawner unityEvent!!

            Damageable damageable = waveEntity.GetComponent<Damageable>();
            if (damageable != null)
            {
                _livingEntities.Add(damageable);
                damageable.CallerDied -= OnEntityDied;
                damageable.CallerDied += OnEntityDied;
            }
        }

        void OnEntityDied(Damageable caller, int currentHealth, int damageTaken)
        {
            _enemiesKilled += 1;
            _livingEntities.Remove(caller);
            caller.CallerDied -= OnEntityDied;

            if (_enemiesKilled == _numberOfEnemiesPerWave[_currentWaveSetIndex])
            {
                WaveStatusChanged?.Invoke(this, SpawnerStatus.Inactive, 0);
                WaveStatusChanged_UnityEvent?.Invoke(this, SpawnerStatus.Inactive, 0);
                WaveStatusEnded.Invoke();
            }
        }

        public void OnEntityReachedBase(Damageable caller)
        {
            //_livingEntities.Remove(caller);

            //if (_enemiesKilled == _numberOfEnemiesPerWave[_currentWaveSetIndex])
            //{
            //    WaveStatusChanged?.Invoke(this, SpawnerStatus.Inactive, 0);
            //    WaveStatusChanged_UnityEvent?.Invoke(this, SpawnerStatus.Inactive, 0);
            //    WaveStatusEnded.Invoke();
            //}
        }

    }
}