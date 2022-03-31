
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Damaging.Scripts
{
    /// <summary>
    /// Simple damageable behaviour implementing health and health changes
    /// </summary>
    public class Damagable : MonoBehaviour
    {
        protected float _currentHealth { get; private set; }
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _startingHealth = 1f;

        public event EventHandler<HealthChangeInfo> _onHealthChanged = null;

        public float NormalisedHealth
        {
            get
            {
                if (Mathf.Abs(_maxHealth) <= Mathf.Epsilon)
                {
                    _maxHealth = 1f;
                }
                return _currentHealth / _maxHealth;
            }
        }

        public bool IsDead => _currentHealth <= 0f;

        public bool IsAtMaxHealth => Mathf.Approximately(_currentHealth, _maxHealth);

        public void Init()
        {
            _currentHealth = _startingHealth;
        }

        public bool TakeDamage(float damageValue, ref HealthChangeInfo healthChangeInfo)
        {
            healthChangeInfo.damageable = this;
            healthChangeInfo.newHealth = _currentHealth;
            healthChangeInfo.oldHealth = _currentHealth;

            if (IsDead)
            {
                return false;
            }

            ChangeHealth(-damageValue, ref healthChangeInfo);

            return true;
        }

        private void ChangeHealth(float damageValue, ref HealthChangeInfo healthChangeInfo)
        {
            healthChangeInfo.oldHealth = _currentHealth;
            _currentHealth += damageValue;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
            healthChangeInfo.newHealth = _currentHealth;

            //call a health changed event here?
            _onHealthChanged?.Invoke(this, healthChangeInfo);
        }
    }

}
