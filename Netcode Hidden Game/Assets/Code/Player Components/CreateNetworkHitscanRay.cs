using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using HiddenGame.ScriptableObjects;

namespace HiddenGame.PlayerComponents
{
    public class CreateNetworkHitscanRay : NetworkBehaviour
    {
        /// <summary>
        /// The camera position and rotation
        /// </summary>
        [SerializeField] private Transform _viewPort;

        /// <summary>
        /// The layers the hitscan will damage or impact
        /// </summary>
        [SerializeField] private LayerMask _layersToHit;

        /// <summary>
        /// The distance the raycast begins from the camera it is fired from
        /// Designed to stop the raycast from hitting the player who fired it
        /// </summary>
        [SerializeField] private float _raycastStartDistance;


        public void FireHitscanRay(HitscanWeapon weaponData)
        {
            //TODO: Implement anti-friendly fire system
            int damage = weaponData.HitscanDamage;
            float range = weaponData.HitscanRange;

            RaycastHit hitscanRaycast;

            if (Physics.Raycast(_viewPort.position, _viewPort.forward, out hitscanRaycast, range, _layersToHit))
            {
                if (hitscanRaycast.transform.TryGetComponent<PlayerHealth>(out PlayerHealth healthScript))
                {
                    healthScript.TakeDamage(damage);
                    healthScript.DebugDamageTaken(damage, hitscanRaycast.transform.gameObject);
                }
            }

            ShowDebugRay(_viewPort.position, _viewPort.position + (_viewPort.forward * range));
        }

        private void ShowDebugRay(Vector3 startPos, Vector3 direction)
        {
            Debug.DrawLine(startPos, direction, Color.red, 2f);

            RpcShowDebugRay(startPos, direction);
        }

        [ClientRpc]
        private void RpcShowDebugRay(Vector3 startPos, Vector3 direction)
        {
            Debug.DrawLine(startPos, direction, Color.red, 2f);
        }
    }
}