
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shooting.Scripts
{

    public class DefaultShootingInstigator : MonoBehaviour, IShootingInstigator
    {
        [SerializeField] private List<AWeapon> _heldWeapons = null;

        private int _activeWeaponIndex = 0;
        private int _weaponSwitchIndex = 0;

        public event OnPerformShot _onPerformShot;

        private float _lastShootTime = 0f;

        protected void Awake()
        {
            if (_heldWeapons != null && _heldWeapons.Count > 0)
            {
                _heldWeapons[0].enabled = true;
            }

            _lastShootTime = 0f;
        }

        public void TryChangeWeapons()
        {
            _heldWeapons[_activeWeaponIndex].enabled = false;

            //cycle through
            _activeWeaponIndex = ++_weaponSwitchIndex % _heldWeapons.Count;
            _heldWeapons[_activeWeaponIndex].enabled = true;

            _lastShootTime = 0f;
        }

        private WeaponPropertiesDefinition ActiveWeaponProperties { get { return _heldWeapons[_activeWeaponIndex].WeaponProperties; } }

        public void TryToShoot(Vector3 aimLocation)
        {
            //anticipate a shoot point when on gamepad or use mouse location?
            //Vector3 targetPos = (transform.forward * 100.0f + transform.position);
            //Debug.DrawLine(transform.position, (transform.forward * 100.0f + transform.position), Color.green, 1f);

            if (_lastShootTime + ActiveWeaponProperties.ShootingInterval <= Time.time)
            {
                _onPerformShot.Invoke(new ShootEventArgs { aimLocation = aimLocation, gunEndPosition = Vector3.zero }); //gunEndPosition unused

                _lastShootTime = Time.time;
            }

        }
    }

}
