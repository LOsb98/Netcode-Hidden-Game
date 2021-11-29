using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiddenGame.Misc;
using HiddenGame.PlayerComponents;

//Inheriting from PlayerController means only one component handling controls is needed per player
//Also means no component references if we want to alter values related to base movement, i.e. movement speed during certain abilities
namespace HiddenGame.Hidden
{
    public class HiddenAbilities : PlayerController
    {
        private float _leapTimer;

        private void Awake()
        {
            Debug.Log("Spawned as hidden");
        }

        private void Update()
        {
            base.Update();
        }

        protected override void FireAbility()
        {
            _movement.BigJump(_camPos.transform.forward);
        }

    }
}
