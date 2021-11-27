using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiddenGame.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Character Data")]
    public class CharacterData : ScriptableObject
    {
        public int Speed;
        public float JumpForce;
    }
}
