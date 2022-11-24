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

        protected override void DoFire()
        {
            var instance = Instantiate(_projectile, _projectileAnchor.position, _projectileAnchor.rotation);
        }

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

        public override void Fire()
        {
            base.Fire();
            _isFiring = true;
        }

    }
}
