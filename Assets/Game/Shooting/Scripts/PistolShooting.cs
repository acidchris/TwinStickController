
using UnityEngine;
using Game.Damaging.Scripts;

namespace Game.Shooting.Scripts
{

    public class PistolShooting : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab = null;
        [SerializeField] private Transform _muzzleRotation = null;

        [SerializeField] private float _damageAmount = 2f;
        [SerializeField] private float _shootInterval = 0.25f;

        private float _lastShootTime = 0f;
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


        protected void OnShoot(object sender, ShootEventArgs eventArgs)
        {
            if (_lastShootTime + _shootInterval <= Time.time)
            {
                _lastShootTime = Time.time;

                GameObject tracerRound = Instantiate(_bulletPrefab, eventArgs.muzzleLoc, _muzzleRotation.rotation);
                var tracerRoundMover = tracerRound.GetComponent<SimpleBulletMover>();
                

                //raycast shooting
                Vector3 shootDirection = (eventArgs.shootPos - eventArgs.muzzleLoc).normalized;
                Ray ray = new Ray(eventArgs.muzzleLoc, shootDirection);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200f))
                {
                    tracerRoundMover.Setup(eventArgs.muzzleLoc, hit.point);

                    IDamageReceiver dmgReceiver = hit.collider.GetComponent<IDamageReceiver>();
                    if (dmgReceiver != null)
                    {
                        dmgReceiver.TakeDamage(_damageAmount, hit.point);
                    }
                }
                else
                {
                    tracerRoundMover.Setup(eventArgs.muzzleLoc, eventArgs.shootPos);
                }

            }
        }

    }

}
