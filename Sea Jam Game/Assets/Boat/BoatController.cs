using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatController : MonoBehaviour
{
    [SerializeField] private float thrust = 100f;
    
    [SerializeField] private Transform rudder = null;
    [Range(1f, 50f)]
    [SerializeField] private float rudderSensitivity = 10f;

    private Rigidbody rb;
    private float currentRudderAngle = 0f;
    private float maxDeflection = 30f;
    private float currentRudderInput;
    bool isThrusting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        InputManager.controls.Vehicle.Enable();
        InputManager.controls.Vehicle.Rudder.performed += ReadRudderInput;
        InputManager.controls.Vehicle.Rudder.canceled += ReadRudderInput;
        InputManager.controls.Vehicle.Thrust.performed += ctx => { isThrusting = ctx.ReadValueAsButton(); };
        InputManager.controls.Vehicle.Thrust.canceled += ctx => { isThrusting = ctx.ReadValueAsButton(); };

        InputManager.controls.Vehicle.Exit.performed += ctx => { if (ctx.ReadValueAsButton()) DismountPlayer(); };
    }
    private void OnDisable()
    {
        
        InputManager.controls.Vehicle.Rudder.performed -= ReadRudderInput;
        InputManager.controls.Vehicle.Rudder.canceled -= ReadRudderInput;
        InputManager.controls.Vehicle.Thrust.performed -= ctx => { isThrusting = ctx.ReadValueAsButton(); };
        InputManager.controls.Vehicle.Thrust.canceled -= ctx => { isThrusting = ctx.ReadValueAsButton(); };
        InputManager.controls.Vehicle.Disable();
    }

    private void Update()
    {
        RespondToRudderInput();
        RespondToTempInput();
        rb.AddTorque(new Vector3(0, currentRudderAngle*rb.velocity.sqrMagnitude, 0));
        Debug.DrawLine(rudder.position, rudder.position + rudder.forward * thrust);
    }

    public void ReadRudderInput(InputAction.CallbackContext ctx)
    {
        currentRudderInput = ctx.ReadValue<float>() * Time.deltaTime;
    }

    private void RespondToRudderInput()
    {
        currentRudderAngle += rudderSensitivity*currentRudderInput;
        currentRudderAngle = Mathf.Clamp(currentRudderAngle, -maxDeflection, maxDeflection);

        rudder.localRotation = Quaternion.Euler(0, currentRudderAngle, 0);
    }
    private void RespondToTempInput()
    {
        if (isThrusting)
        {
            float boost = Vector3.Angle(
                Vector3.ProjectOnPlane(transform.up, Vector3.up),
                Vector3.ProjectOnPlane(transform.forward, Vector3.up)
            );
            boost = 1+Mathf.Lerp(4, 0, Mathf.InverseLerp(0, 50, boost));
            print(boost);
            rb.AddForce(transform.forward*thrust*boost);
        }

    }

    private void DismountPlayer()
    {
        FindObjectOfType<RigidbodyCharacterController>().Dismount();
    }
}
