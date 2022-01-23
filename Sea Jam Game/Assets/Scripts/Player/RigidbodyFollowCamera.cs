using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFollowCamera : MonoBehaviour
{
    public Transform target;
    [SerializeField] float minDist = 10f;
    [SerializeField] float maxDist = 30f;
    [SerializeField] float topSpeed = 50f;
    [SerializeField] float forwardLookFactor = 20f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            rb = target.GetComponent<Rigidbody>();
        }
        catch
        {
            Debug.LogError(name + " can't follow target " + target.name + " as it isn't a Rigidbody.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // determine follow rail start & end ('close' units behind targets direction, to 'far' units behind targets velocity)
        Vector3 closePoint = -target.transform.forward.normalized * minDist;
        Vector3 farPoint = -rb.velocity.normalized * maxDist;

        //TODO ground detection

        // place the camera at appropriate place along follow rail
        transform.position = target.transform.position + Vector3.Lerp(closePoint, farPoint, rb.velocity.magnitude / topSpeed);

        // point camera towards target plus a bit in the direction it's looking

        transform.LookAt(target.transform.position + forwardLookFactor * rb.velocity.magnitude * target.transform.forward, target.transform.up);
    }
}
