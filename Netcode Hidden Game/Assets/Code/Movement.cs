using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public void GroundMove(Rigidbody rb, Vector3 moveVector, float speed)
    {
        //Setting velocity directly makes ground movement more consistent
        //Reduces how many physics factors can affect movement
        rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
    }

    public void AirMove(Rigidbody rb, Vector3 moveVector, float speed)
    {
        if (rb.velocity.magnitude < speed)
        {
            rb.AddForce(moveVector, ForceMode.Acceleration);
        }

        //Clamping velocity is needed when using AddForce()
        //Stops player gaining infinite speed
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -speed, speed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -speed, speed));
    }

    public void Jump(Rigidbody rb, float jumpForce)
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }
}
