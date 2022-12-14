namespace GSGD1
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "GameSup/Tower Description", fileName ="TowerDescription")]
	public class TowerDescription : ScriptableObject
	{
		[SerializeField]
		private Tower _prefab = null;

		[SerializeField]
		private Sprite _icon = null;

		[SerializeField]
		private Color _iconColor = Color.white;

        [SerializeField]
        private int _cookieCost = 0;

        [SerializeField]
        private List<TowerDescription> _upgradeList = new List<TowerDescription>();

        public Tower Prefab => _prefab;
		public Sprite Icon => _icon;
		public Color IconColor => _iconColor;
        public int CookieCost => _cookieCost;
        public List<TowerDescription> UpgradeList => _upgradeList;

        public Tower Instantiate(Tower instantiatedTower)
		{
			return GameObject.Instantiate(instantiatedTower);
		}
	}
}