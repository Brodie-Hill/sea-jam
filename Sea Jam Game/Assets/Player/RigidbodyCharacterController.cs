using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidbodyCharacterController : MonoBehaviour
{

    // Info
    private float diameter = 0, height = 0;


    // Input
    public InputMaster controls = null;
    private Vector2 previousWalkInput = Vector2.zero;


    // Movement
    [Header("Movement (Translation)")]
    [SerializeField] private float walkForce = 100f;
    [SerializeField] private float maxWalk = 2f, maxRun = 4.5f/*, maxCrouch = 0.7f*/;
    [SerializeField] private float jumpForce = 1000f;
    [SerializeField] private float airControl = 0.3f;
    [SerializeField] private LayerMask groundLayer = new LayerMask();
    [SerializeField] private float groundDetectionDistance = 0.05f;

    private bool isGrounded = true, isRunning = false;
    private RaycastHit groundHitInfo;
    private Vector3 currentWalkForce = Vector3.zero;
    private float currentMaxSpeed = 1.5f;
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
    private MeshCollider wallSliderCol = null;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new InputMaster();
        collider = GetComponent<CapsuleCollider>();
        wallSliderCol = GetComponentInChildren<MeshCollider>();
        currentMount = null;
        rigidbody = GetComponent<Rigidbody>();
        diameter = GetComponent<CapsuleCollider>().radius * 2;
        height = GetComponent<CapsuleCollider>().height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentMount == null)
        {
            CalculateLocalXZ();
            CheckGrounded();
            RespondToTranslationInput();
            RespondToRotationInput();
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        
        controls.Player.Walk.performed += ReadWalkInput;
        controls.Player.Walk.canceled += ReadWalkInput;
        controls.Player.Run.performed += ReadRunInput;
        controls.Player.Run.canceled += ReadRunInput;
        controls.Player.Jump.performed += ReadJumpInput;
        controls.Player.Jump.canceled += ReadJumpInput;

        controls.Player.Look.performed += ReadMouseInput;
        controls.Player.Look.canceled += ReadMouseInput;

        controls.Player.Interact.performed += ReadInteractInput;

        controls.Enable();
        
    }
    private void OnDisable()
    {
        controls.Player.Walk.performed -= ReadWalkInput;
        controls.Player.Walk.canceled -= ReadWalkInput;
        controls.Player.Run.performed -= ReadRunInput;
        controls.Player.Run.canceled -= ReadRunInput;
        controls.Player.Jump.performed -= ReadJumpInput;
        controls.Player.Jump.canceled -= ReadJumpInput;

        controls.Player.Look.performed -= ReadMouseInput;
        controls.Player.Look.canceled -= ReadMouseInput;

        controls.Player.Interact.performed -= ReadInteractInput;

        controls.Disable();
    }

    public void ReadWalkInput(InputAction.CallbackContext ctx)
    {
        previousWalkInput = ctx.ReadValue<Vector2>();

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

        if (previousWalkInput != Vector2.zero)
        {
            currentWalkForce =
                forward * previousWalkInput.y * walkForce +
                right * previousWalkInput.x * walkForce;

            rigidbody.AddForce(currentWalkForce * (Mathf.Clamp(Mathf.Pow(currentMaxSpeed, 2) - rigidbody.velocity.sqrMagnitude, 0, currentMaxSpeed)) * (isGrounded ? 1 : airControl));

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
            isGrounded = true;

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
        if (currentMount == null)
        {
            // turn off all the colliders
            collider.enabled = false;
            wallSliderCol.enabled = false;
            rigidbody.isKinematic = true;
        }
        currentMount = mount;
        transform.position = mount.GetMountPoint().position;
        transform.rotation = mount.GetMountPoint().rotation;

        mount.OnMounted?.Invoke();
    }
    public void Dismount()
    {
        // turn collider back on
        collider.enabled = true;
        wallSliderCol.enabled = true;
        rigidbody.isKinematic = false;

        transform.position = currentMount.GetDismountPoint().position;
        transform.rotation = currentMount.GetDismountPoint().rotation;
        currentMount = null;

        currentMount?.OnDismounted?.Invoke();
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
        // look directions
        Vector3 camPos = Camera.main.transform.position;
        //Debug.DrawLine(camPos, camPos + head.forward, Color.yellow);
    }
}
