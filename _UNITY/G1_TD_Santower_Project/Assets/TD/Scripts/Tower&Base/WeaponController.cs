namespace GSGD1
{
	using UnityEngine;

	public class WeaponController : MonoBehaviour
	{
		[SerializeField]
		private AWeapon _weapon = null;

		[SerializeField]
		private float _rotationSpeed = 5f;

		[SerializeField]
		private float _minAngleToFire = 10f;

		[Header("Artillery")]
		[SerializeField]
		private bool _useArtilleryAiming = false;
		[SerializeField]
		private float _artilleryAimHeight = 10f;

		[Header("Parts")]
		[SerializeField]
		private TowerBase _towerBase;
		[SerializeField]
		private TowerCannon _towerCannon;
		[SerializeField]
		private bool _onlyCanonRotates = false;

		private DamageableDetector _damageableDetector;

		[System.NonSerialized]
		private Quaternion _lastLookRotation = Quaternion.identity;

		private void Awake()
		{
			_damageableDetector = GetComponentInParent<DamageableDetector>();
		}

		public virtual void LookAt(Vector3 position)
		{
            Vector3 direction = (position - transform.position).normalized;

            _towerBase.BaseLookAt(position, _rotationSpeed, _onlyCanonRotates);

            if (_useArtilleryAiming)
			{
				direction = ((position + new Vector3(0, _artilleryAimHeight, 0)) - transform.position).normalized;
				_towerCannon.CannonLookAt(direction, _rotationSpeed, _onlyCanonRotates);
			}

            _towerCannon.CannonLookAt(direction, _rotationSpeed, _onlyCanonRotates);
            
			
			
			//_lastLookRotation = Quaternion.LookRotation(direction, Vector3.up);
   //         transform.rotation = Quaternion.Slerp(transform.rotation, _lastLookRotation, _rotationSpeed * Time.deltaTime);

   //         _weapon.AnchorLookAt(position);
		}

		public void Fire()
		{
			_weapon.Fire();
		}

		public void LookAtAndFire(Vector3 position)
		{
			LookAt(position);
			if (Quaternion.Angle(_lastLookRotation, transform.rotation) < _minAngleToFire)
			{
				Fire();
			}
		}

		private void LateUpdate()
		{
			_lastLookRotation = transform.rotation;
		}

		public void UpgradeRotationSpeed(float value)
		{
			_rotationSpeed = value;
		}
	}
}