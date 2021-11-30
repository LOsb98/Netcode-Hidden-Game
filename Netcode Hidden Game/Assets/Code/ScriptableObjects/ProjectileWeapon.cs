using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiddenGame.PlayerComponents;

namespace HiddenGame.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Weapon Data/Projectile Weapon")]
    public class ProjectileWeapon : WeaponData
    {
        [SerializeField] private CreateNetworkProjectile _projectileScript;

        public GameObject ProjectileToFire;

        public override void Initialize(GameObject thisObject)
        {
            _projectileScript = thisObject.GetComponent<CreateNetworkProjectile>();
        }

        public override void Fire()
        {
            
        }
    }
}
