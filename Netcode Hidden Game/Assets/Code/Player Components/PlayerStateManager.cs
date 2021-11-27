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

        //[SyncVar(hook = nameof(SetPlayerActive))] [SerializeField] private bool _isSpawned;

        [SerializeField] private CharacterData _humanData;
        [SerializeField] private CharacterData _hiddenData;

        [SerializeField] private HumanAbilities _humanScript;
        [SerializeField] private HiddenAbilities _hiddenScript;

        [SerializeField] private MeshRenderer _bodyCapsuleMesh;
        [SerializeField] private Material _humanMaterial;
        [SerializeField] private Material _hiddenMaterial;

        [SerializeField] private Movement _moveScript;

        private void Start()
        {

        }

        public override void OnStartClient()
        {
            //If this is not the local player, we can delete these components
            //The game (so far) is client authoritative for movement, so we don't care about what these scritps want to do
            if (!isLocalPlayer)
            {
                Destroy(_moveScript);
                Destroy(_humanScript);
                Destroy(_hiddenScript);
                Destroy(GetComponent<Rigidbody>());
            }

            //Calling hook method here to make sure visuals are synced
            InitializeCharacter(!_isHidden, _isHidden);
        }

        private void InitializeCharacter(bool oldRole, bool newRole)
        {
            //Hook method for setting data/visuals
            SetCharacterMaterial(newRole);

            if (isLocalPlayer)
            {
                //SetCharacterData(_isHidden);
            }
        }

        private void SetCharacterMaterial(bool characterIsHidden)
        {
            if (characterIsHidden)
            {
                _bodyCapsuleMesh.material = _hiddenMaterial;
            }
            else
            {
                _bodyCapsuleMesh.material = _humanMaterial;
            }

            if (!_bodyCapsuleMesh.enabled)
            {
                _bodyCapsuleMesh.enabled = true;
            }
        }

        public void SetCharacterData(bool characterIsHidden)
        {
            if (characterIsHidden)
            {
                Debug.Log("Selected hidden player role");

                _humanScript.enabled = false;
                _hiddenScript.enabled = true;
                _moveScript.SetData(_hiddenData);
            }
            else
            {
                Debug.Log("Selected human player role");

                _hiddenScript.enabled = false;
                _humanScript.enabled = true;
                _moveScript.SetData(_humanData);
            }
        }

        private void SetPlayerActive(bool oldState, bool newState)
        {
            gameObject.SetActive(newState);
        }

        [Command]
        public void CmdServerSetRole(bool isPlayerHidden)
        {
            _isHidden = isPlayerHidden;
            //InitializeCharacter(!_isHidden, _isHidden);
        }

        //[Command]
        //public void CmdSetPlayerSpawned(bool isPlayerSpawned)
        //{
        //    _isSpawned = isPlayerSpawned;
        //    SetPlayerActive(false, _isSpawned);
        //}
    }
}
