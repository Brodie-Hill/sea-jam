using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightCycle : MonoBehaviour
{
    [SerializeField] private Light sun = null;
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        rotationAxis.Normalize();
        sun.transform.rotation = Quaternion.LookRotation(rotationAxis, Vector3.up);
        sun.transform.Rotate(90, 0, 0, Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        sun.transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
