
using UnityEngine;
using UnityEngine.Pool;
using Game.Damaging.Scripts;
using System;

namespace Game.Shooting.Scripts
{

    public class PistolShooting : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab = null;
        [SerializeField] private Transform _bulletContainer = null;
        [SerializeField] private Transform _muzzleRotation = null;

        [SerializeField] private float _damageAmount = 2f;
        [SerializeField] private float _shootInterval = 0.25f;



        private IObjectPool<ABullet> _bulletPool = null;

        private float _lastShootTime = 0f;
        private IShootingInstigator _instigator = null;

        protected void Awake()
        {
            _bulletPool = new ObjectPool<ABullet>(OnCreatePooledObject, OnTakeFromPool, OnReturnToPool);

            _instigator = GetComponent<IShootingInstigator>();
            _instigator.OnShoot += OnShoot;
        }

        protected void OnDestroy()
        {
            _instigator.OnShoot -= OnShoot;

            _bulletPool.Clear();
        }

        protected void OnShoot(object sender, ShootEventArgs eventArgs)
        {
            if (_lastShootTime + _shootInterval <= Time.time)
            {
                _lastShootTime = Time.time;

                var bullet = _bulletPool.Get();
                var tracerRoundMover = bullet.GetComponent<SimpleBulletMover>();
                tracerRoundMover.transform.SetPositionAndRotation(eventArgs.muzzleLoc, _muzzleRotation.rotation);
                
                //raycast shooting
                Vector3 shootDirection = (eventArgs.shootPos - eventArgs.muzzleLoc).normalized;
                Ray ray = new Ray(eventArgs.muzzleLoc, shootDirection);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200f))
                {
                    tracerRoundMover.SetupBullet(eventArgs.muzzleLoc, hit.point);

                    IDamageReceiver dmgReceiver = hit.collider.GetComponent<IDamageReceiver>();
                    if (dmgReceiver != null)
                    {
                        dmgReceiver.TakeDamage(_damageAmount, hit.point);
                    }
                }
                else
                {
                    tracerRoundMover.SetupBullet(eventArgs.muzzleLoc, eventArgs.shootPos);
                }

            }
        }

        private ABullet OnCreatePooledObject()
        {
            GameObject go = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity);
            var bullet = go.GetComponent<ABullet>();
            bullet.SetPool(_bulletPool);

            bullet.transform.SetParent(_bulletContainer);

            return bullet;
        }

        private void OnTakeFromPool(ABullet bullet)
        {
            bullet.gameObject.SetActive(true);
        }

        private void OnReturnToPool(ABullet bullet)
        {
            bullet.gameObject.SetActive(false);
        }

    }

}
