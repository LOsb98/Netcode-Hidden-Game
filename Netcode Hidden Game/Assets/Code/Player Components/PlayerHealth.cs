using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using HiddenGame.ScriptableObjects;
using HiddenGame.Misc;

namespace HiddenGame.PlayerComponents
{
    public class PlayerHealth : NetworkBehaviour
    {
        [SerializeField] private GameObject _healthBar;

        private Slider _healthSlider;

        private Transform _healthBarTransform;

        private Transform _localPlayer;

        [SyncVar] private int _maxHealth;
        [SyncVar(hook = nameof(UpdateHealthBar))] private int _currentHealth;

        private void Awake()
        {
            _healthSlider = _healthBar.GetComponent<Slider>();
            _healthBarTransform = _healthBar.transform;
        }

        private void Start()
        {
            //if (isServer)
            //{
            //    return;
            //}

            //_localPlayer = NetworkClient.localPlayer.transform;

            if (isLocalPlayer)
            {
                _healthBar.gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            // Make health bars always face toward the main camera
            _healthBarTransform.LookAt(CameraFollow.Instance.transform);
        }

        public void SetMaxHealth(int newHealth)
        {
            _healthSlider.maxValue = newHealth;
            _maxHealth = newHealth;
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            _currentHealth -= damageAmount;
        }

        public void HealHealth(int healAmount)
        {
            _currentHealth += healAmount;
        }

        private void UpdateHealthBar(int oldValue, int newValue)
        {
            if (newValue > _maxHealth)
            {
                _healthSlider.value = _maxHealth;
            }
            else
            {
                _healthSlider.value = newValue;
            }
        }
    }
}
