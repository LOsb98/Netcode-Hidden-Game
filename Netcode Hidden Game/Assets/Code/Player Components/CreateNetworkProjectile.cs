using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace HiddenGame.PlayerComponents
{
    public class CreateNetworkProjectile : NetworkBehaviour
    {
        /// <summary>
        /// The camera position and rotation
        /// </summary>
        [SerializeField] private Transform _viewPortPosition;

        [Command]
        public void CmdCreateProjectileOnNetwork(GameObject projectile)
        {

        }
    }
}
