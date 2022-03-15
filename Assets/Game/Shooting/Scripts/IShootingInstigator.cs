using System;
using UnityEngine;


namespace Game.Shooting.Scripts
{
    public class ShootEventArgs
    {
        public Vector3 shootPos;
        public Vector3 muzzleLoc;
    }

    public interface IShootingInstigator
    {


        public event EventHandler<ShootEventArgs> OnShoot;


        void DoShoot(Vector3 shootFromPos);
    }

}
