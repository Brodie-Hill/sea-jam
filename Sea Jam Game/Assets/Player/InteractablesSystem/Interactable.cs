using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public static List<Interactable> Interactables { get; } = new List<Interactable>();

    public float range { get; private set; } = 2f;

    //public UnityEvent OnEnterProximity;
    public UnityEvent OnLookOver;
    public UnityEvent<Interactor> OnInteract;
    public UnityEvent OnLookAway;
    //public UnityEvent OnExitProximity;

    private void OnEnable()
    {
        Interactables.Add(this);
    }
    private void OnDisable()
    {
        OnLookAway.Invoke();
        Interactables.Remove(this);
    }

}
