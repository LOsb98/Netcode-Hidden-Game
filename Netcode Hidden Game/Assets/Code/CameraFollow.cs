using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _followPos;

    private static CameraFollow _instance;

    public static CameraFollow Instance
    {
        get
        {
            return _instance;
        }
    }

    public Transform FollowPos
    {
        get
        {
            return _followPos;
        }
        set
        {
            _followPos = value;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void LateUpdate()
    {
        //Using Lerp() instead of setting transform.position makes camera movement much smoother
        //Lerp() created problems with viewmodels when they existed as children of the WeaponPos object, jittered around a lot
        //Using Lerp() + an overlap camera for weapon viewmodels is the best combination so far

        transform.position = Vector3.Lerp(transform.position, _followPos.position, 0.3f);
        transform.rotation = _followPos.rotation;
    }
}
