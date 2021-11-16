using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouyantBody : MonoBehaviour
{
    [Header("Fluid Properties")]
    [Tooltip("Density of fluid this bouyantBody is floating in [kg/m^3]")]
    [SerializeField] private float fluidDensity = 997f;
    [SerializeField] private float fluidDragLinear = 5f;
    [SerializeField] private float fluidDragAngular = 3f;

    [Header("Bouyancy Properties")]
    [SerializeField] private BouyantPoint[] bouyantPoints = null;

    

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        foreach (BouyantPoint bouyantPoint in bouyantPoints)
        {
            Vector3 force = bouyantPoint.GetBouyantForce(fluidDensity);
            rb.AddForceAtPosition(force, bouyantPoint.transform.position);
            rb.AddForce(fluidDragLinear * bouyantPoint.GetSubmergeAmount() * -rb.velocity);
            rb.AddTorque(fluidDragAngular * bouyantPoint.GetSubmergeAmount() * -rb.angularVelocity);
        }
    }
}
