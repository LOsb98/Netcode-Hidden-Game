using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HiddenGame.Misc;
using HiddenGame.PlayerComponents;
using Mirror;

//Inheriting from PlayerController means only one component handling controls is needed per player
//Also means no component references if we want to alter values related to base movement, i.e. movement speed during certain abilities
namespace HiddenGame.Hidden
{
    public class HiddenAbilities : PlayerController
    {
        [SerializeField] private float _meleeSize;
        [SerializeField] private int _meleeDamage;
        [SerializeField] private Transform _meleeTransform;
        [SerializeField] private LayerMask _meleeHitLayers;

        private void Awake()
        {
            Debug.Log("Spawned as hidden");
        }

        private void Update()
        {
            base.Update();

            if (Input.GetMouseButtonDown(0) && isLocalPlayer)
            {
                CmdMeleeAttack();
            }
        }

        protected override void FireAbility()
        {
            _movement.BigJump(_camPos.transform.forward);
        }

        [Command]
        private void CmdMeleeAttack()
        {
            Collider[] meleeHits = Physics.OverlapSphere(_meleeTransform.position, _meleeSize, _meleeHitLayers);

            //Save a list of objects this attack has already hit
            List<GameObject> objectsAlreadyHit = new List<GameObject>();

            foreach (Collider hit in meleeHits)
            {
                GameObject hitObject = hit.gameObject;

                if (hitObject == gameObject)
                {
                    //Stop hitting yourself
                    Debug.Log("Stop hitting yourself");
                    continue;
                }

                if (objectsAlreadyHit.Contains(hitObject))
                {
                    //Players have 2 colliders, this avoids them taking damage twice from melee, once for each collider
                    Debug.Log("List already contains object");
                    continue;
                }

                if (hitObject.TryGetComponent<PlayerHealth>(out PlayerHealth healthScript))
                {
                    healthScript.TakeDamage(_meleeDamage);
                    healthScript.DebugDamageTaken(_meleeDamage, hitObject);

                    objectsAlreadyHit.Add(hitObject);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.blue;

            Gizmos.DrawSphere(_meleeTransform.position, _meleeSize);
        }

    }
}
