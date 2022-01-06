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

        public override void OnStartClient()
        {
            base.OnStartClient();

            UpdateHealthBar(_currentHealth, _currentHealth);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            UpdateHealthBar(_currentHealth, _currentHealth);
        }

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

            if (_currentHealth <= 0)
            {
                //Respawn player
                //TODO: Respawn manager currently works locally on client, needs to be changed to handle respawns from server end
            }
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

        public void DebugDamageTaken(int damageTaken, GameObject playerHit)
        {
            Debug.Log($"{playerHit} was hit for {damageTaken} damage.");

            RpcDebugDamageTaken(damageTaken, playerHit);
        }

        [ClientRpc]
        private void RpcDebugDamageTaken(int damageTaken, GameObject playerHit)
        {
            Debug.Log($"{playerHit} was hit for {damageTaken} damage.");
        }
    }
}
