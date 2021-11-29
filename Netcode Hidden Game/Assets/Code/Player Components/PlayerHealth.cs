using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using HiddenGame.ScriptableObjects;

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
            _localPlayer = NetworkClient.localPlayer.transform;

            if (isLocalPlayer)
            {
                _healthBar.gameObject.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            _healthBarTransform.LookAt(_localPlayer.transform);
        }

        [Command]
        public void CmdSetMaxHealth(int newHealth)
        {
            _healthSlider.maxValue = newHealth;
            _maxHealth = newHealth;
            _currentHealth = _maxHealth;
        }

        [Command]
        public void CmdChangeHealth(int healthChangeValue)
        {
            _currentHealth += healthChangeValue;
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
