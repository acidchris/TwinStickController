using System;
using UnityEngine;


namespace Game.Shooting.Scripts
{
    public class ShootEventArgs : EventArgs
    {
        public Vector3 aimLocation;
        public Vector3 gunEndPosition;
    }

    public delegate void OnPerformShot(ShootEventArgs args);


    public interface IShootingInstigator
    {
        public event OnPerformShot _onPerformShot;

        /// <summary>
        /// Does not necessarily mean that we actually fire a shot.
        /// Mainly forward the "shoot" input
        /// </summary>
        /// <param name="aimLocation"></param>
        void TryToShoot(Vector3 aimLocation);

        void TryChangeWeapons();

    }

}
