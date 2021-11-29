using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiddenGame.Misc;
using HiddenGame.PlayerComponents;
using Mirror;

//Inheriting from PlayerController means only one component handling controls is needed per player
//Also means no component references if we want to alter values related to base movement, i.e. movement speed during certain abilities
namespace HiddenGame.Human
{
    public class HumanAbilities : PlayerController
    {
        [SerializeField] private GameObject _landmine;

        private void Awake()
        {
            Debug.Log("Spawned as human");
        }

        private void Update()
        {
            base.Update();
        }

        protected override void FireAbility()
        {
            CmdSpawnLandmine();
        }

        [Command]
        private void CmdSpawnLandmine()
        {
            GameObject newMine = Instantiate(_landmine, transform.position, Quaternion.Euler(0f, 0f, 0f));
            NetworkServer.Spawn(newMine);
        }

    }
}
