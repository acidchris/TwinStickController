using UnityEngine;


namespace Game.Damaging.Scripts
{
    public struct HealthChangeInfo
    {
        public Damagable damageable;
        public float oldHealth;

        public float newHealth;

        public float healthDifference
        {
            get { return newHealth - oldHealth; }
        }

        public float absHealthDifference
        {
            get { return Mathf.Abs(healthDifference); }
        }
    }

}
