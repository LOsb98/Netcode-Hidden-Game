using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiddenGame.Misc;
using HiddenGame.PlayerComponents;

//Inheriting from PlayerController means only one component handling controls is needed per player
//Also means no component references if we want to alter values related to base movement, i.e. movement speed during certain abilities
namespace HiddenGame.Human
{
    public class HumanAbilities : PlayerController
    {
        private void Awake()
        {
            Debug.Log("Spawned as human");
        }

        private void Update()
        {
            base.Update();
        }

    }
}
