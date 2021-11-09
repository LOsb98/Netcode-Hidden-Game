using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public void GroundMove(Rigidbody rb, Vector3 moveVector, float speed)
    {
        //Setting velocity directly makes ground movement more consistent
        //Reduces how many physics factors can affect movement
        rb.velocity = new Vector3(moveVector.x, -1f, moveVector.z);
    }

    public void AirMove(Rigidbody rb, Vector3 moveVector, float speed)
    {
        if (rb.velocity.magnitude <= speed)
        {
            rb.AddForce(moveVector, ForceMode.Acceleration);
        }

        float maxAxisSpeed = speed / 2;

        //Clamping velocity is needed when using AddForce()
        //Stops player gaining infinite speed
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -maxAxisSpeed, maxAxisSpeed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -maxAxisSpeed, maxAxisSpeed));
    }

    public void Jump(Rigidbody rb, float jumpForce)
    {
        transform.position += new Vector3(0f, 0.2f, 0f);
        rb.velocity += new Vector3(0, jumpForce, 0);
    }
}
