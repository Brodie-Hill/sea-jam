using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mountable : MonoBehaviour
{
    [SerializeField] private Transform mountPoint = null;
    [SerializeField] private Transform dismountPoint = null;
    [SerializeField] public UnityEvent OnMounted = null;
    [SerializeField] public UnityEvent OnDismounted = null;

    public void Mount()
    {
        FindObjectOfType<RigidbodyCharacterController>().SetMount(this); // only one player so this works
    }
    public Transform GetMountPoint() { return mountPoint; }
    public Transform GetDismountPoint() { return dismountPoint; }
}
