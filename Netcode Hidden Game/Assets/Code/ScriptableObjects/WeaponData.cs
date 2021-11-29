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

        public abstract void Initialize();

        public abstract void Fire();
    }
}
