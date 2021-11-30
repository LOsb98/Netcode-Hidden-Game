using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using HiddenGame.Hidden;
using HiddenGame.Human;
using HiddenGame.ScriptableObjects;

namespace HiddenGame.PlayerComponents
{
    public class PlayerStateManager : NetworkBehaviour
    {
        [SyncVar(hook = nameof(SetTeamRole))] private bool _isHidden;

        [SyncVar(hook = nameof(SetPlayerActive))] private bool _isSpawned;

        [SerializeField] private CharacterData _humanData;
        [SerializeField] private CharacterData _hiddenData;

        [SerializeField] private HumanAbilities _humanScript;
        [SerializeField] private HiddenAbilities _hiddenScript;

        [SerializeField] private MeshRenderer _bodyCapsuleMesh;

        [SerializeField] private Movement _moveScript;
        [SerializeField] private PlayerHealth _healthScript;

        private PlayerController _activeController;

        public override void OnStartClient()
        {
            base.OnStartClient();

            SetTeamRole(!_isHidden, _isHidden);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            SetTeamRole(!_isHidden, _isHidden);
        }

        private void Start()
        {
            if (!isLocalPlayer)
            {
                Destroy(GetComponent<Rigidbody>());
            }

            //SetTeamRole(!_isHidden, _isHidden);
        }

        private void SetTeamRole(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                int maxHealth = _hiddenData.Health;
                _healthScript.SetMaxHealth(maxHealth);

                Material hiddenMaterial = _hiddenData.CharacterMaterial;
                _bodyCapsuleMesh.material = hiddenMaterial;

                _activeController = _hiddenScript;

                _moveScript.SetData(_hiddenData);
            }
            else
            {
                int maxHealth = _humanData.Health;
                _healthScript.SetMaxHealth(maxHealth);

                Material hiddenMaterial = _humanData.CharacterMaterial;
                _bodyCapsuleMesh.material = hiddenMaterial;

                _activeController = _humanScript;

                _moveScript.SetData(_humanData);
            }
        }

        public void AssignRole(bool isHidden)
        {
            if (isHidden)
            {
                CmdJoinHiddenTeam();
            }
            else
            {
                CmdJoinHumanTeam();
            }
        }

        [Command]
        private void CmdJoinHiddenTeam()
        {
            _isHidden = true;

            EnablePlayer();
        }

        [Command]
        private void CmdJoinHumanTeam()
        {
            _isHidden = false;

            EnablePlayer();
        }

        private void EnablePlayer()
        {
            _isSpawned = true;
        }

        private void SetPlayerActive(bool oldValue, bool newValue)
        {
            _activeController.enabled = newValue;
            _bodyCapsuleMesh.enabled = newValue;
        }
    }
}
