using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputMaster controls;
    public static InputManager singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
        {
            Debug.Log("InputManager singleton: destroying duplicate instance of WaveManager");
            Destroy(this);
        }

        controls = new InputMaster();
        //controls.Enable();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void FreeMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
