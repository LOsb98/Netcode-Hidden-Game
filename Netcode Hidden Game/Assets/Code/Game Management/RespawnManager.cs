using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace HiddenGame.GameManagement
{
    public class RespawnManager : MonoBehaviour
    {
        public Transform[] _respawnPositions;

        private static RespawnManager _instance;

        public static RespawnManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RespawnPlayer();
            }
        }

        public void RespawnPlayer()
        {
            int spawnIndex = Random.Range(0, _respawnPositions.Length - 1);

            Transform localPlayer = NetworkClient.localPlayer.transform;

            localPlayer.position = _respawnPositions[spawnIndex].position;
            localPlayer.rotation = _respawnPositions[spawnIndex].rotation;
        }
    }
}