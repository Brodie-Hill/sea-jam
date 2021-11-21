using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    [SerializeField]
    private InputAction action;

    [SerializeField]
    private CinemachineVirtualCamera vcam1; // player camera

    [SerializeField]
    private CinemachineVirtualCamera vcam2; // boat camera

    private Animator animator;

    private bool PlayerCamera = true;

    private void Awake()
    {
       // animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }
    void Start()
    {
        action.performed += _ => SwitchPriority(); //SwitchState();
    }

    private void SwitchState()
    {
        if (PlayerCamera)
        {
            animator.Play("PlayerCamera");
        }
        else
        {
            animator.Play("BoatCamera");
        }
        PlayerCamera = !PlayerCamera;
    }
   
    private void SwitchPriority()
    {
        if(PlayerCamera)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
        PlayerCamera = !PlayerCamera;
    }
}
