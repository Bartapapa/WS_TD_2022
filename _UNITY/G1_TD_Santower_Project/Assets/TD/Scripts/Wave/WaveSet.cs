namespace GSGD1
{
	using System.Collections.Generic;
	using UnityEngine;

#if UNITY_EDITOR
	using UnityEditor;
#endif //UNITY_EDITOR

	[System.Serializable]
	public class WaveEntityGroupDescriptionField
	{
		[SerializeField]
		private WaveEntityGroupDescription _waveEntityGroupDescription;

		[SerializeField]
		private int _spawnerIndex;

		[SerializeField]
		private int _waypointOverride = -1;

		public WaveEntityGroupDescription GetWEGD => _waveEntityGroupDescription;

		public int Spawner => _spawnerIndex;

		public int WaypointOverride => _waypointOverride;
	}

	[CreateAssetMenu(menuName = "GameSup/WaveSet")]
	public class WaveSet : ScriptableObject
	{
		[SerializeField]
		private List<WaveEntityGroupDescriptionField> _waves = null;	

		[SerializeField]
		private float _waitingDurationBefore = 0f;

		[SerializeField]
		private float _waitingDurationAfter = 5f;

		public List<WaveEntityGroupDescriptionField> Waves => _waves;

		public float WaitingDurationBefore => _waitingDurationBefore;
		public float WaitingDurationAfter => _waitingDurationAfter;

		public float GetWaveDuration()
		{
			float duration = 0;
			for (int i = 0, length = _waves.Count; i < length; i++)
			{
				foreach (Wave wave in _waves[i].GetWEGD.GetWaves)
				{ 
					duration += wave.GetWaveDuration();
				}
			}
			return duration + _waitingDurationBefore + _waitingDurationAfter;
		}
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(WaveSet))]
	public class WaveSetEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			serializedObject.Update();

			var waveDatabase = (serializedObject.targetObject as WaveSet);

			EditorGUILayout.Space(24);
			EditorGUILayout.TextArea(string.Format("Duration : {0} seconds", waveDatabase.GetWaveDuration().ToString()));
		}
	}
#endif //UNITY_EDITOR

}