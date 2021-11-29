using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiddenGame.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Weapon Data/Hitscan Weapon")]
    public abstract class HitscanWeapon : WeaponData
    {
        public override void Initialize()
        {
            //Get component for creating a raycast
        }

        public override void Fire()
        {

        }
    }
}
