
using UnityEngine;
using Game.Damaging.Scripts;
using System;

namespace Game.Enemies.Scripts
{
    public class EnemyDamageReceiver : MonoBehaviour, IDamageReceiver
    {
        [SerializeField] private DamagableBehaviour _damagableBehaviour = null;

        public event EventHandler<HealthChangeInfo> _onDied = null;

        void Start()
        {
            _damagableBehaviour.Init();
        }

        void OnDestroy()
        {

        }

        public void TakeDamage(float baseAmount, Vector3 hitPoint)
        {
            HealthChangeInfo info = new HealthChangeInfo();
            _damagableBehaviour.TakeDamage(baseAmount, ref info);

            if (_damagableBehaviour.IsDead)
            {
                _onDied?.Invoke(this, info);
            }
        }

    }

}
