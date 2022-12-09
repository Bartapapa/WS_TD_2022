namespace GSGD1
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
		private AnimatorHandler_Tower _anim = null;

		[SerializeField]
		private Damageable _damageable = null;

        [SerializeField]
        //Readonly
        private TowerDescription _towerDescription = null;

        [SerializeField]
        //Readonly
        private int _totalCookieCost = 0;

		[SerializeField]
		private bool _enabledByDefault = false;

		[SerializeField]
		private bool _canBeSelectedByDefault = false;

        public TowerDescription TowerDescription => _towerDescription;
        public int GetTotalCookieCost => _totalCookieCost;

		public DamageableDetector damageableDetector => _damageableDetector;

        protected virtual void Awake()
		{
			enabled = _enabledByDefault;
			_selectableObject.SetCanBeSelected(_canBeSelectedByDefault);

			_damageable = GetComponent<Damageable>();

            if (_anim == null)
            {
                Debug.Log(name + " doesn't have an animatorHandler. Please advise.");
            }
            else
            {
                _anim.Initialize();
            }
        }

		public void Enable(bool isEnabled)
		{
			enabled = isEnabled;
            _selectableObject.SetCanBeSelected(isEnabled);

			if (isEnabled)
			{
				if (_anim != null)
				{
                    _anim.Animator.SetTrigger("Spawn");
                }

            }

			else
			{
				if (_anim != null)
				{
                    _anim.Animator.SetTrigger("Die");
                }

            }
        }

		private void Update()
		{
			if (_damageableDetector == null) return;

			if (_damageableDetector.HasAnyDamageableInRange() == true)
			{
				Damageable damageableTarget = _damageableDetector.GetTarget();
				//_weaponController.LookAt(damageableTarget.GetAimPosition());
				//_weaponController.Fire();

				_weaponController.LookAtAndFire(damageableTarget.GetAimPosition());
				if (damageableTarget == null)
				{
                    _damageableDetector.RemoveNullItemsFromList();
                }
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

		public void KillTower()
		{
			Enable(false);
			_selectableObject.SetCanBeSelected(false);
			if (_damageable != null)
			{
				_damageable.Die();
			}
			else
			{
				Destroy(this.gameObject);
			}
		}
    }
}