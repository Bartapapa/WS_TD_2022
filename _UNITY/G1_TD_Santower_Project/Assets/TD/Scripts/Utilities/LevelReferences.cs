namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class LevelReferences : Singleton<LevelReferences>
	{
		[SerializeField]
		private PlayerPickerController _playerPickerController = null;

		[SerializeField]
		private SpawnerManager _spawnerManager = null;

		[SerializeField]
		private Damageable _northPole = null;

		public PlayerPickerController PlayerPickerController => _playerPickerController;
		public SpawnerManager SpawnerManager => _spawnerManager;

		public Damageable NorthPole => _northPole;
	}
}