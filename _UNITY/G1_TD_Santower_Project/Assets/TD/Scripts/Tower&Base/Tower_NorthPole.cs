namespace GSGD1
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Tower_NorthPole : Tower
    {
        [SerializeField]
        private WeaponController _weaponController2;

        protected override void Awake()
        {
            Enable(true);
			_weaponController2.enabled = false;
        }

		private void LateUpdate()
		{
			if (damageableDetector.HasAnyDamageableInRange() == true)
			{
				Damageable damageableTarget = damageableDetector.GetSecondDamageable();
				//_weaponController.LookAt(damageableTarget.GetAimPosition());
				//_weaponController.Fire();

				_weaponController2.LookAtAndFire(damageableTarget.GetAimPosition());
				if (damageableTarget == null)
				{
					damageableDetector.RemoveNullItemsFromList();
				}
			}
		}
		public void DoubleGatling()
		{
			_weaponController2.enabled = true;
		}
	}
}

