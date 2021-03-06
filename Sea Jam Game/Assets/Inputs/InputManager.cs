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

    // Update is called once per frame
    void Update()
    {
        
    }
}
