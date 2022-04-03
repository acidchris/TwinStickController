
using Game.Damaging.Scripts;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Shooting.Scripts
{
    public class TracerRoundShootingWeapon : AWeapon
    {
        [SerializeField] private GameObject _bulletPrefab = null;
        [SerializeField] private Transform _bulletContainer = null;
        [SerializeField] protected Transform _muzzleRotation = null;  //should we expose this into weapon properties?

        [SerializeField] private GameObject _weaponObject = null;

        private IObjectPool<ABullet> _bulletPool = null;

        private IShootingInstigator _instigator = null;

        protected virtual void Awake()
        {
            if (_instigator == null)
            {
                _instigator = GetComponent<IShootingInstigator>();
            }

            Debug.Assert(_instigator != null);

            _bulletPool = new ObjectPool<ABullet>(OnCreatePooledObject, OnTakeFromPool, OnReturnToPool);
        }

        protected virtual void OnEnable()
        {
            _instigator._onPerformShot += OnFireShotInternally;

            _weaponObject.SetActive(true);
        }

        protected virtual void OnDisable()
        {
            _instigator._onPerformShot -= OnFireShotInternally;

            _weaponObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            _instigator._onPerformShot -= OnFireShotInternally;
            _bulletPool.Clear();
            _bulletPool = null;
        }


        protected virtual void OnFireShotInternally(ShootEventArgs args)
        {

            ABullet bullet = _bulletPool.Get();
            SimpleBulletMover tracerRoundMover = bullet.GetComponent<SimpleBulletMover>();
            tracerRoundMover.transform.SetPositionAndRotation(_muzzleRotation.position, _muzzleRotation.rotation);

            //raycast shooting
            float spread = UnityEngine.Random.Range(-WeaponProperties.BulletSpreadFactor, WeaponProperties.BulletSpreadFactor);
            Vector3 spreadShootPos = args.aimLocation + new Vector3(spread, 0f, spread);

            Vector3 shootDirection = (spreadShootPos - _muzzleRotation.position).normalized;
            Ray ray = new Ray(_muzzleRotation.position, shootDirection);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200f))
            {
                tracerRoundMover.SetupBullet(_muzzleRotation.position, hit.point);

                IDamageReceiver dmgReceiver = hit.collider.GetComponent<IDamageReceiver>();
                if (dmgReceiver != null)
                {
                    dmgReceiver.TakeDamage(WeaponProperties.BaseDamage, hit.point);
                }
            }
            else
            {
                tracerRoundMover.SetupBullet(_muzzleRotation.position, spreadShootPos/*eventArgs.shootPos*/);
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
