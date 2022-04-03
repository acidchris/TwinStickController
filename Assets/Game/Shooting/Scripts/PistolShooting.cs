
namespace Game.Shooting.Scripts
{
    public sealed class PistolShooting : TracerRoundShootingWeapon { }


#if _PISTOL_DEPRECADED
    public class PistolShooting : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab = null;
        [SerializeField] private Transform _bulletContainer = null;
        [SerializeField] private Transform _muzzleRotation = null;

        [SerializeField] private GameObject _weaponObject = null;

        [SerializeField] private float _damageAmount = 2f;
        [SerializeField] private float _shootInterval = 0.25f;

        [SerializeField, Range(0f, 1.0f)] private float _bulletSpreadFactor = 0f;   //bullet spread between -factor to +factor (randomized for x and z position)



        private IObjectPool<ABullet> _bulletPool = null;

        private float _lastShootTime = 0f;
        private IShootingInstigator _instigator = null;

        protected void Awake()
        {
            _bulletPool = new ObjectPool<ABullet>(OnCreatePooledObject, OnTakeFromPool, OnReturnToPool);

            _instigator = GetComponent<IShootingInstigator>();
        }

        protected void OnEnable()
        {
            _weaponObject.SetActive(true);
            _instigator._onPerformShot += OnShoot;
        }

        protected void OnDisable()
        {
            _weaponObject.SetActive(false);
            _instigator._onPerformShot -= OnShoot;
        }

        protected void OnDestroy()
        {
            _bulletPool.Clear();
            _bulletPool = null;
        }

        protected void OnShoot(ShootEventArgs eventArgs)
        {
            if (_lastShootTime + _shootInterval <= Time.time)
            {
                _lastShootTime = Time.time;

                ABullet bullet = _bulletPool.Get();
                SimpleBulletMover tracerRoundMover = bullet.GetComponent<SimpleBulletMover>();
                tracerRoundMover.transform.SetPositionAndRotation(_muzzleRotation.position, _muzzleRotation.rotation);

                //raycast shooting
                float spread = UnityEngine.Random.Range(-_bulletSpreadFactor, _bulletSpreadFactor);
                Vector3 spreadShootPos = eventArgs.aimLocation + new Vector3(spread, 0f, spread);

                Vector3 shootDirection = (spreadShootPos - _muzzleRotation.position).normalized;
                Ray ray = new Ray(_muzzleRotation.position, shootDirection);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200f))
                {
                    tracerRoundMover.SetupBullet(_muzzleRotation.position, hit.point);

                    IDamageReceiver dmgReceiver = hit.collider.GetComponent<IDamageReceiver>();
                    if (dmgReceiver != null)
                    {
                        dmgReceiver.TakeDamage(_damageAmount, hit.point);
                    }
                }
                else
                {
                    tracerRoundMover.SetupBullet(_muzzleRotation.position, spreadShootPos/*eventArgs.shootPos*/);
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
#endif
}
