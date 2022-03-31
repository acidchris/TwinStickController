
using UnityEngine;
using Game.Damaging.Scripts;

namespace Game.Enemies.Scripts
{

    public class EnemyMeleeAttack : AEnemyAttack
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            _playerTrackingModule = GameStatics.QueryGameModule<PlayerTrackingGameModule>(nameof(PlayerTrackingGameModule));
        }

        protected override void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.transform == _playerTrackingModule.PlayerTransform)
            {
                _playerDamageReceiver = otherCollider.GetComponentInChildren<IDamageReceiver>();
                _playerInRange = true;
            }
        }

        protected override void OnTriggerExit(Collider otherCollider)
        {
            if (otherCollider.transform == _playerTrackingModule.PlayerTransform)
            {
                _playerDamageReceiver = null;
                _playerInRange = false;
            }
        }

        private void Update()
        {
            if (_playerInRange && _playerDamageReceiver != null)
            {
                //play anim

                //apply continuously some damage
                _playerDamageReceiver.TakeDamage(0.1f, Vector3.zero);
            }
        }

        private bool _playerInRange = false;
        private IDamageReceiver _playerDamageReceiver = null;

        private PlayerTrackingGameModule _playerTrackingModule = null;
    }

}
