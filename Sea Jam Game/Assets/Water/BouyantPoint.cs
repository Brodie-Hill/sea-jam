using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouyantPoint : MonoBehaviour
{
    [Tooltip("How deep until this floater is fully submerged, where its bouyant force is at a maximum")]
    [SerializeField] private float submergedDepth = 1f;

    [Tooltip("How much water is displaced when fully submerged [m^3]")]
    [SerializeField] private float displacementAmount = 3f;

    private float currentSubmergeAmount;
    private Color submergedColor;
    private Color surfacedColor;


    private void Awake()
    {
        submergedColor = new Color(0.2f, 0.3f, 0.5f, 1);
        surfacedColor = new Color(0.7f, 0.8f, 1f, 1);
}
    public Vector3 GetBouyantForce(float fluidDensity)
    {
        Vector3 bouyantForce = Vector3.zero;
        float waveHeight = WaveManager.singleton.GetWaveHeightAtPosition(transform.position);
        float depth = waveHeight - transform.position.y; // positive value == underwater, negative == above
        if (depth > 0)
        {
            // do bouyancy calc
            currentSubmergeAmount = Mathf.Clamp01(depth / submergedDepth);
            float dispMult = displacementAmount * currentSubmergeAmount;
            bouyantForce = -Physics.gravity*fluidDensity * dispMult;
        }

        return bouyantForce;
    }

    public float GetSubmergeAmount()
    {
        return currentSubmergeAmount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.Lerp(surfacedColor, submergedColor, currentSubmergeAmount);
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}