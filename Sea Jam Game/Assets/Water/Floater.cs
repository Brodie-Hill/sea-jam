using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    
    [SerializeField] private Rigidbody rigidbody = null;

    [Tooltip("How deep until this floater is fully submerged")]
    [SerializeField] private float submergedDepth = 1f;

    [Tooltip("How much water is displaced when fully submerged")]
    [SerializeField] private float displacementAmount = 3f;

    [SerializeField] private float waterLinearDrag = 0.7f;
    [SerializeField] private float waterAngularDrag = 0.3f;

    private void FixedUpdate()
    {
        float waveHeight = WaveManager.singleton.GetWaveHeightAtPosition(transform.position);
        float depth = waveHeight - transform.position.y; // positive value == underwater, negative == above
        if (depth>0)
        {
            // do bouyancy
            float dispMult = Mathf.Clamp01(depth / submergedDepth) / displacementAmount;
            rigidbody.AddForceAtPosition(-Physics.gravity * dispMult, transform.position, ForceMode.Acceleration);
            rigidbody.AddForce(dispMult * -rigidbody.velocity * waterLinearDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidbody.AddTorque(dispMult * -rigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

}
