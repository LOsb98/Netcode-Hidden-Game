using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiddenGame.PlayerComponents;

namespace HiddenGame.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Weapon Data/Hitscan Weapon")]
    public class HitscanWeapon : WeaponData
    {
        [SerializeField] private CreateNetworkHitscanRay _raycastScript;

        public float HitscanRange;

        public int HitscanDamage;

        public override void Initialize(GameObject thisObject)
        {
            _raycastScript = thisObject.GetComponent<CreateNetworkHitscanRay>();
        }

        public override void Fire()
        {
            //_raycastScript.FireHitscanRay(HitscanDamage, HitscanRange);
        }
    }
}
