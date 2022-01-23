using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyCharacterController : MonoBehaviour
{

    // Info
    private float diameter = 0, height = 0;


    // Input
    private Vector2 currentWalkInput = Vector2.zero;

    // Animation

    // Movement
    [Header("Movement (Translation)")]
    [SerializeField] private float walkForce = 1000f;
    [SerializeField] private float stoppingForce = 500f;
    [SerializeField] private float maxWalk = 2f, maxRun = 4.5f/*, maxCrouch = 0.7f*/;
    [SerializeField] private float maxSlope = 45;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float airControl = 0.3f;
    [SerializeField] private LayerMask groundLayer = new LayerMask();
    [SerializeField] private float groundDetectionDistance = 0.05f;

    private bool isGrounded = true, isRunning = false;
    private RaycastHit groundHitInfo;
    private Vector3 currentWalkForce = Vector3.zero;
    private float currentMaxSpeed = 1.5f;
    private float minNormalY;
    private Rigidbody rigidbody;
    private Vector3 forward, right;


    // Rotation
    [Header("Movement (Rotation)")]
    [SerializeField] private Vector2 lookSensitivity = Vector2.zero;
    [SerializeField] private float neckRange = 180;
    [SerializeField] private Transform head = null;
    private Vector2 currentMouseDelta = Vector2.zero;
    private float xRot = 0, yRot = 0;

    // Mounting
    public Mountable currentMount { get; private set; } = null;
    private CapsuleCollider collider = null;
    private Interactor interactor = null;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        currentMount = null;
        rigidbody = GetComponent<Rigidbody>();
        diameter = GetComponent<CapsuleCollider>().radius * 2;
        height = GetComponent<CapsuleCollider>().height;

        interactor = GetComponent<Interactor>();

        minNormalY = Mathf.Abs(1*Mathf.Sin(maxSlope));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (currentMount == null)
        {
            CalculateLocalXZ();
            CheckGrounded();
            rigidbody.useGravity = !isGrounded;
            RespondToTranslationInput();
            RespondToRotationInput();
        }
        else
        {
            transform.position = currentMount.GetMountPoint().position;
            transform.rotation = currentMount.GetMountPoint().rotation;
        }
    }

    private void OnEnable()
    {
        InputManager.singleton.LockMouse();

        InputManager.controls.Player.Walk.performed += ReadWalkInput;
        InputManager.controls.Player.Walk.canceled += ReadWalkInput;
        InputManager.controls.Player.Run.performed += ReadRunInput;
        InputManager.controls.Player.Run.canceled += ReadRunInput;
        InputManager.controls.Player.Jump.performed += ReadJumpInput;
        InputManager.controls.Player.Jump.canceled += ReadJumpInput;

        InputManager.controls.Player.Look.performed += ReadMouseInput;
        InputManager.controls.Player.Look.canceled += ReadMouseInput;

        InputManager.controls.Player.Interact.performed += ReadInteractInput;
        InputManager.controls.Player.Enable();

        InputManager.controls.Vehicle.Exit.performed += ReadDismountInput;
    }
    private void OnDisable()
    {
        InputManager.controls.Player.Walk.performed -= ReadWalkInput;
        InputManager.controls.Player.Walk.canceled -= ReadWalkInput;
        InputManager.controls.Player.Run.performed -= ReadRunInput;
        InputManager.controls.Player.Run.canceled -= ReadRunInput;
        InputManager.controls.Player.Jump.performed -= ReadJumpInput;
        InputManager.controls.Player.Jump.canceled -= ReadJumpInput;

        InputManager.controls.Player.Look.performed -= ReadMouseInput;
        InputManager.controls.Player.Look.canceled -= ReadMouseInput;

        InputManager.controls.Player.Interact.performed -= ReadInteractInput;

        InputManager.controls.Player.Disable();

        InputManager.controls.Vehicle.Exit.performed -= ReadDismountInput;
    }

    public void ReadWalkInput(InputAction.CallbackContext ctx)
    {
        currentWalkInput = ctx.ReadValue<Vector2>();

    }
    public void ReadRunInput(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.ReadValueAsButton();

    }
    public void ReadMouseInput(InputAction.CallbackContext ctx)
    {
        currentMouseDelta = ctx.ReadValue<Vector2>();
    }
    public void ReadJumpInput(InputAction.CallbackContext ctx)
    {
        if (isGrounded && ctx.ReadValueAsButton())
        {
            rigidbody.AddForce(Vector3.up * jumpForce);
        }
    }
    public void ReadInteractInput(InputAction.CallbackContext ctx)
    {
        if (currentMount == null)
            GetComponent<Interactor>()?.Interact();
        else Dismount();
    }


    private void RespondToTranslationInput()
    {
        currentMaxSpeed = isRunning ? maxRun : maxWalk;

        if (currentWalkInput != Vector2.zero)
        {
            currentWalkForce =
                forward * currentWalkInput.y * walkForce +
                right * currentWalkInput.x * walkForce;

            rigidbody.AddForce(currentWalkForce * (Mathf.Clamp(Mathf.Pow(currentMaxSpeed, 2) - rigidbody.velocity.sqrMagnitude, 0, currentMaxSpeed)) * (isGrounded ? 1 : airControl));
        }
        if (isGrounded)
        {
            rigidbody.AddForce(stoppingForce * -rigidbody.velocity);
        }

    }
    private void RespondToRotationInput()
    {
        xRot -= currentMouseDelta.y*lookSensitivity.y;
        yRot += currentMouseDelta.x*lookSensitivity.x;

        xRot = Mathf.Clamp(xRot, -neckRange, neckRange);
        
        transform.rotation = Quaternion.Euler(0, yRot, 0);
        head.localRotation = Quaternion.Euler(xRot, 0, 0);

    }

    void CheckGrounded()
    {
        float sphereCastRadius = diameter / 2 - 0.02f;
        
        if (Physics.SphereCast(transform.position+transform.up*height/2, sphereCastRadius, -transform.up, out groundHitInfo, height / 2 - sphereCastRadius + groundDetectionDistance, groundLayer))
        {
            isGrounded = groundHitInfo.normal.y > minNormalY;

        }
        else
        {
            isGrounded = false;
        }
    }

    void CalculateLocalXZ()
    {
        if (isGrounded)
        {
            forward = Vector3.Cross(groundHitInfo.normal, -transform.right);
            right = Vector3.Cross(groundHitInfo.normal, transform.forward);
        }
        else
        {

            forward = transform.forward;
            right = transform.right;
        }

    }

    public void SetMount(Mountable mount)
    {
        StartCoroutine(Mount(mount));
    }
    private IEnumerator Mount(Mountable mount)
    {
        if (currentMount == null)
        {
            // turn off all the colliders and interactions
            collider.enabled = false;
            rigidbody.isKinematic = true;
            interactor.enabled = false;
        }
        
        InputManager.controls.Player.Disable();
        mount.OnMounted?.Invoke();
        yield return new WaitForSeconds(mount.GetMountTime());
        currentMount = mount;
    }
    public void ReadDismountInput(InputAction.CallbackContext ctx)
    {
        Dismount();
    }
    public void Dismount()
    {
        if (currentMount == null) return;

        // turn collider and interactor back on
        collider.enabled = true;
        rigidbody.isKinematic = false;
        interactor.enabled = true;

        transform.position = currentMount.GetDismountPoint().position;
        transform.rotation = currentMount.GetDismountPoint().rotation;
        

        InputManager.controls.Player.Enable();
        currentMount?.OnDismounted?.Invoke();

        currentMount = null;
    }

    private void OnDrawGizmos()
    {
        // directions
        Debug.DrawLine(transform.position, transform.position + forward, Color.blue);
        Debug.DrawLine(transform.position, transform.position + right, Color.red);
        Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);
        // ground hit
        if (isGrounded)
        {
            Gizmos.DrawSphere(groundHitInfo.point, 0.1f);
            Debug.DrawLine(transform.position, transform.position + groundHitInfo.normal * 0.2f, Color.black);
        }
        // movement directions
        Debug.DrawLine(transform.position, transform.position + currentWalkForce.normalized, Color.cyan);
        if (rigidbody != null)
            Debug.DrawLine(transform.position, transform.position + rigidbody.velocity.normalized, Color.magenta);
    }


}
