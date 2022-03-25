using UnityEngine;

namespace Game.Damaging.Scripts
{

    public interface IDamageReceiver
    {
        void TakeDamage(float baseAmount, Vector3 hitPoint);
    }

}
