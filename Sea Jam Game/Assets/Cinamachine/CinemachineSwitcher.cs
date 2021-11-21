using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{
    public static CinemachineSwitcher singleton;

    [SerializeField]
    private CinemachineVirtualCamera vcam1; // player camera

    [SerializeField]
    private CinemachineVirtualCamera vcam2; // boat camera


    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
        {
            Debug.Log("InputManager singleton: destroying duplicate instance of WaveManager");
            Destroy(this);
        }
    }

    public void SwitchToOne()
    {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
    }
    public void SwitchToTwo()
    {
        vcam2.Priority = 1;
        vcam1.Priority = 0;
    }
}
