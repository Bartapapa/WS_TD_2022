namespace GSGD1
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SantaGatling : ProjectileLauncher
    {
        [SerializeField] private float _continueFiringDuration;
        [SerializeField] private bool _isFiring = false;
        private float _continueFiringTimer = 0f;

        protected override void Update()
        {
            base.Update();
            if (_isFiring)
            {
                if (_continueFiringTimer < _continueFiringDuration)
                {
                    _continueFiringTimer += Time.deltaTime;
                    Fire();
                }
                else
                {
                    _continueFiringTimer = 0f;
                    _isFiring = false;
                }
            }
        }

        public override void AnchorLookAt(Vector3 position)
        {
            //TODO force anchor to follow WeaponController.forward.

            //Vector3 lookPos = position - transform.position;
            //Quaternion lookRotation = Quaternion.LookRotation(lookPos);
            //_projectileAnchor.rotation = lookRotation;
        }

        public override void Fire()
        {
            base.Fire();
            _isFiring = true;
        }

        public void GatlingUpgrade()
        {
            _timer.Set(0.15f);
        }

        //public override void AnchorLookAt(Vector3 position)
        //{
        //    //TODO force anchor to follow WeaponController.forward.

        //    //Vector3 lookPos = position - transform.position;
        //    //Quaternion lookRotation = Quaternion.LookRotation(lookPos);
        //    //_projectileAnchor.rotation = lookRotation;
        //}

    }
}

