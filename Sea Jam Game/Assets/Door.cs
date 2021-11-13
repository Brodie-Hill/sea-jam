using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private bool isOpen;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = isOpen;
        GetComponent<BoxCollider>().enabled = isOpen;
    }
    public void Toggle()
    {
        isOpen = !isOpen;
        GetComponent<MeshRenderer>().enabled = isOpen;
        GetComponent<BoxCollider>().enabled = isOpen;
    }

}
