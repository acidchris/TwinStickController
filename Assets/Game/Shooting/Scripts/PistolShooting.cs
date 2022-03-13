using System;
using UnityEngine;

namespace Game.Shooting.Scripts
{

    public class PistolShooting : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab = null;
        [SerializeField] private Transform _muzzleLocation = null;

        private float _lastShootTime = 0f;
        private float _nextShootTime = 0.1f;

        private IShootingInstigator _instigator = null;

        protected void Awake()
        {
            _instigator = GetComponent<IShootingInstigator>();
            _instigator.OnShoot += OnShoot;
        }

        protected void OnDestroy()
        {
            _instigator.OnShoot -= OnShoot;
        }


        protected void OnShoot(object sender, float v)
        {
            if (_lastShootTime + _nextShootTime <= Time.time)
            {
                _lastShootTime = Time.time;

                //shoot

                GameObject g = Instantiate(_bulletPrefab, _muzzleLocation.position, _muzzleLocation.rotation);

                Destroy(g, 2);
            }
        }

    }

}