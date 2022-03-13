using System;

using UnityEngine;

namespace Game.Shooting.Scripts
{

    public class DefaultShootingInstigator : MonoBehaviour, IShootingInstigator
    {
        public event EventHandler<float> OnShoot;

        public void DoShoot(float v)
        {
            OnShoot?.Invoke(this, v);
        }

    }

}
