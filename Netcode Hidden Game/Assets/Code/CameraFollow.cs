using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followPos;

    void LateUpdate()
    {
        //Using Lerp() instead of setting transform.position makes camera movement much smoother
        //Lerp() created problems with viewmodels when they existed as children of the WeaponPos object, jittered around a lot
        //Using Lerp() + an overlap camera for weapon viewmodels is the best combination so far
        transform.position = Vector3.Lerp(transform.position, followPos.position, 0.3f);
        transform.rotation = followPos.rotation;
    }
}
