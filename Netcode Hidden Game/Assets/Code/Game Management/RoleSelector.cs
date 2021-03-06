using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using HiddenGame.PlayerComponents;
using TMPro;

namespace HiddenGame.GameManagement
{
    public class RoleSelector : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentRoleText;

        [SerializeField] private bool _isPlayerHidden;

        public void AssignRole(bool isPlayerHidden)
        {
            _isPlayerHidden = isPlayerHidden;

            string newText = "Error?";

            if (_isPlayerHidden)
            {
                newText = "Selected hidden role";
            }
            else
            {
                newText = "Selected human role";
            }

            _currentRoleText.text = newText;
        }

        public void SpawnPlayerAsRole()
        {
            GameObject localPlayer = NetworkClient.localPlayer.gameObject;

            PlayerStateManager stateManager = localPlayer.GetComponent<PlayerStateManager>();

            stateManager.CmdServerSetRole(_isPlayerHidden);

            stateManager.CmdSetPlayerSpawned(true);

            RespawnManager.Instance.RespawnPlayer();

            gameObject.SetActive(false);
        }
    }
}