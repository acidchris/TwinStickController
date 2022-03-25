using Game.Damaging.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies.Scripts
{

    public class EnemyHealthVisualizer : MonoBehaviour
    {
        [SerializeField] private DamagableBehaviour _damagableBehaviour = null;

        [SerializeField] private Transform _backgroundBar = null;
        [SerializeField] private Transform _healthBar = null;
        [SerializeField] private bool _showWhenAtFullHealth = false;

        private Transform _mainCamera = null;



        private void Awake()
        {
            _damagableBehaviour._onHealthChanged += OnHealthChanged;
            _mainCamera = Camera.main.transform;

            gameObject.SetActive(_showWhenAtFullHealth);
        }

        private void OnDestroy()
        {
            _damagableBehaviour._onHealthChanged -= OnHealthChanged;
        }

        private void Update()
        {
            Vector3 direction = _mainCamera.transform.forward;

            transform.forward = -direction;
        }

        private void OnHealthChanged(object sender, HealthChangeInfo e)
        {
            UpdateHealth(e.damageable.NormalisedHealth);
        }

        private void UpdateHealth(float normalizedHealth)
        {
            Vector3 scale = Vector3.one;

            if (_healthBar != null)
            {
                scale.x = normalizedHealth;
                _healthBar.transform.localScale = scale;
            }

            if (_backgroundBar != null)
            {
                scale.x = 1 - normalizedHealth;
                _backgroundBar.transform.localScale = scale;
            }

            gameObject.SetActive(_showWhenAtFullHealth || normalizedHealth < 1.0f);
        }
    }

}
