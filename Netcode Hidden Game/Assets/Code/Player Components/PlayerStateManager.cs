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
        [SyncVar(hook = nameof(InitializeCharacter))] private bool _isHidden;

        [SyncVar(hook = nameof(SetPlayerActive))] private bool _isSpawned;

        [SerializeField] private CharacterData _humanData;
        [SerializeField] private CharacterData _hiddenData;

        [SerializeField] private HumanAbilities _humanScript;
        [SerializeField] private HiddenAbilities _hiddenScript;

        [SerializeField] private MeshRenderer _bodyCapsuleMesh;
        [SerializeField] private Material _humanMaterial;
        [SerializeField] private Material _hiddenMaterial;

        [SerializeField] private Movement _moveScript;
        [SerializeField] private PlayerHealth _healthScript;

        private PlayerController _activeController;

        private void Start()
        {
            if (!isLocalPlayer)
            {
                Destroy(GetComponent<Rigidbody>());
            }
        }

        public override void OnStartServer()
        {
            InitializeCharacter(!_isHidden, _isHidden);
            SetPlayerActive(!_isSpawned, _isSpawned);
        }

        public override void OnStartClient()
        {
            InitializeCharacter(!_isHidden, _isHidden);
            SetPlayerActive(!_isSpawned, _isSpawned);
        }

        private void InitializeCharacter(bool oldRole, bool newRole)
        {
            //Hook method for setting data/visuals
            
            if (newRole)
            {
                SetCharacterData(_hiddenData);
                SetCharacterMaterial(_hiddenData);
            }
            else
            {
                SetCharacterData(_humanData);
                SetCharacterMaterial(_humanData);
            }
        }

        private void SetCharacterMaterial(CharacterData newCharData)
        {
            _bodyCapsuleMesh.material = newCharData.CharacterMaterial;

            if (!_bodyCapsuleMesh.enabled)
            {
                _bodyCapsuleMesh.enabled = true;
            }
        }

        public void SetCharacterData(CharacterData newCharData)
        {
            Debug.Log("Setting data");

            // Mirror only allows certain data types to be passed into Commands
            // Since the scriptable object for character data uses a material now,
            // the individual values have to be passed separately where needed,
            // instead of the whole scriptable object data container
            if (_isHidden)
            {
                _activeController = _hiddenScript;
            }
            else
            {
                _activeController = _humanScript;
            }

            _moveScript.SetData(newCharData);
            _healthScript.CmdSetMaxHealth(newCharData.Health);

        }

        private void SetPlayerActive(bool oldState, bool newState)
        {
            _activeController.enabled = newState;
        }

        [Command]
        public void CmdServerSetRole(bool isPlayerHidden)
        {
            _isHidden = isPlayerHidden;
            InitializeCharacter(!_isHidden, _isHidden);
        }

        [Command]
        public void CmdSetPlayerSpawned(bool isPlayerSpawned)
        {
            _isSpawned = isPlayerSpawned;
            SetPlayerActive(!_isSpawned, _isSpawned);
        }
    }
}
