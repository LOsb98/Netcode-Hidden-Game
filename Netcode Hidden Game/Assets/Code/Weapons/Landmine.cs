using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Weapons
{
    public class Landmine : NetworkBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private BoxCollider _collider;

        private void Start()
        {
            RemoveComponents();
        }

        private void RemoveComponents()
        {
            // Removing unecessary physics on the client end
            if (!isServer)
            {
                Destroy(_rb);
                Destroy(_collider);
                Destroy(this);
            }
        }
    }
}
