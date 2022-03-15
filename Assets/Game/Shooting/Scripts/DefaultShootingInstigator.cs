using System;

using UnityEngine;

namespace Game.Shooting.Scripts
{

    public class DefaultShootingInstigator : MonoBehaviour, IShootingInstigator
    {

        [SerializeField] private Transform _muzzleLocation = null;


        public event EventHandler<ShootEventArgs> OnShoot;

        public void DoShoot(Vector3 shootFromPos)
        {
            //anticipate a shoot point when on gamepad or use mouse location?
            //Vector3 targetPos = (transform.forward * 100.0f + transform.position);
            //Debug.DrawLine(transform.position, (transform.forward * 100.0f + transform.position), Color.green, 1f);

            OnShoot?.Invoke(this, new ShootEventArgs(){shootPos = shootFromPos/*targetPos*/, muzzleLoc = _muzzleLocation.position});
        }

    }

}
