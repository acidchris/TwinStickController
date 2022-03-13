using System;


namespace Game.Shooting.Scripts
{

    public interface IShootingInstigator
    {

        public event EventHandler<float> OnShoot;


        void DoShoot(float v);
    }

}
