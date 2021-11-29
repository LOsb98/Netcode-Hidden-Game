using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiddenGame.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Weapon Data/Projectile Weapon")]
    public abstract class ProjectileWeapon : WeaponData
    {
        public override void Initialize()
        {
            //Get component for instantiating a projectile
        }

        public override void Fire()
        {
            
        }
    }
}
