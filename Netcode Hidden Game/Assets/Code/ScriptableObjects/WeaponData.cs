using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiddenGame.ScriptableObjects
{
    public abstract class WeaponData : ScriptableObject
    {
        public GameObject ViewModel;
        public int ClipSize;
        public float FireRate;
        public float ReloadTime;

        public abstract void Initialize(GameObject thisObject);

        /// <summary>
        /// Shoots the relvant weapon.
        /// This will always be done on the server to 
        /// prevent players cheating in their own damage values.
        /// </summary>
        public abstract void Fire();
    }
}
