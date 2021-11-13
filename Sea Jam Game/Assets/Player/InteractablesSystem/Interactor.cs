using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField] private RawImage crossHairFocusEffect = null;
    [SerializeField] private LayerMask interactableMask = new LayerMask();
    [SerializeField] private Transform rayOrigin = null;
    Interactable focus = null;

    // Update is called once per frame
    void Update()
    {
        Interactable lookingAt = null;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, 10, interactableMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.TryGetComponent(out Interactable i))
            {
                if (hit.distance <= i.range)
                {
                    lookingAt = i;
                    
                }
                
            }
        }

        if (lookingAt == null && focus != null)
        {
            focus.OnLookAway?.Invoke();
            focus = null;
            crossHairFocusEffect.gameObject.SetActive(false);
            return;
        }
        if (focus!=lookingAt)
        {
            
            focus?.OnLookAway?.Invoke();
            focus = lookingAt;
            focus.OnLookOver?.Invoke();
            crossHairFocusEffect.gameObject.SetActive(true);
        }

        
    }
    public void Interact()
    {
        focus?.OnInteract?.Invoke();
    }
}
