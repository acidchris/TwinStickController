
using Game.Damaging.Scripts;
using System;
using UnityEngine;

namespace Game.Player.Scripts
{
    public class PlayerDamageReceiver : MonoBehaviour, IDamageReceiver
    {
        [SerializeField] private Damagable _damagable = null;

        public event EventHandler<HealthChangeInfo> _onDied = null;

        public void TakeDamage(float baseAmount, Vector3 hitPoint)
        {
            HealthChangeInfo healthChangeInfo = new HealthChangeInfo();
            _damagable.TakeDamage(baseAmount, ref healthChangeInfo);

            if (_damagable.IsDead)
            {
                _onDied?.Invoke(this, healthChangeInfo);
            }
        }

        private void Awake()
        {
            var t = transform.parent; //FIXXMEE with unique class or so to refer to
            PlayerTrackingGameModule playerTracking = new PlayerTrackingGameModule(t);
            GameStatics.AddGameModule<PlayerTrackingGameModule>(nameof(PlayerTrackingGameModule), playerTracking);

            _damagable.Init();
        }

        private void OnDestroy()
        {
            GameStatics.RemoveGameModule<PlayerTrackingGameModule>(nameof(PlayerTrackingGameModule));
        }
    }

}
