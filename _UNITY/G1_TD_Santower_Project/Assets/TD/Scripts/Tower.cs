﻿namespace GSGD1
{
	using GSGD1;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Facade for Tower subsystems
	/// </summary>
	public class Tower : MonoBehaviour, IPickerGhost, ICellChild, IPlateChild
	{
        [SerializeField]
		private WeaponController _weaponController = null;

		[SerializeField]
		private DamageableDetector _damageableDetector = null;

		[SerializeField]
		private SelectableObject _selectableObject = null;

        [SerializeField]
        //Readonly
        private TowerDescription _towerDescription = null;

        [SerializeField]
        //Readonly
        private int _totalCookieCost = 0;

        public TowerDescription TowerDescription => _towerDescription;
        public int GetTotalCookieCost => _totalCookieCost;

        private void Awake()
		{
			enabled = false;
			_selectableObject.SetCanBeSelected(false);
		}

		public void Enable(bool isEnabled)
		{
			enabled = isEnabled;
            _selectableObject.SetCanBeSelected(true);
        }

		private void Update()
		{
			if (_damageableDetector.HasAnyDamageableInRange() == true)
			{
				Damageable damageableTarget = _damageableDetector.GetNearestDamageable();
				//_weaponController.LookAt(damageableTarget.GetAimPosition());
				//_weaponController.Fire();

				_weaponController.LookAtAndFire(damageableTarget.GetAimPosition());
			}
		}

        public void SetTotalCookieCost(int value)
        {
            _totalCookieCost = value;
            if (_totalCookieCost <= 0)
            {
                _totalCookieCost = 0;
            }
        }

        // Interfaces
        public Transform GetTransform()
		{
			return transform;
		}

		public void OnSetChild()
		{
			Enable(true);
			_selectableObject.SetCanBeSelected(true);
		}

        public Transform GetParent()
        {
            return transform.parent;
        }

        public Cell GetCell()
        {
            return transform.parent.gameObject.GetComponent<Cell>();
        }
    }
}