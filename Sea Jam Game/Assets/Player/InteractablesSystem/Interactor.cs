using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField] private RawImage crosshairFocusEffect = null;
    [SerializeField] private Text crosshairInfoText = null;
    [SerializeField] private LayerMask interactableMask = new LayerMask();
    [SerializeField] private Transform rayOrigin = null;

    Interactable focus = null;
    bool crosshairEffectsEnabled;

    private void Start()
    {
        focus = null;
        DisableCrosshairEffects();

    }
    
    void Update()
    {
        Interactable lookingAt = null;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, 10, interactableMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.TryGetComponent(out Interactable i))
            {
                if (hit.distance <= i.range)
                    lookingAt = i;
            }
        }

        if (lookingAt!=null && focus == null && !crosshairEffectsEnabled)
        {
            // looked at any interactable
            focus = lookingAt;
            focus.OnLookOver.Invoke();
            EnableCrosshairEffects(focus.name);
        }
        else if (lookingAt==null && crosshairEffectsEnabled)
        {
            // stopped looking at any interactable
            focus?.OnLookAway.Invoke();
            focus = null;
            DisableCrosshairEffects();
        }
        else if (lookingAt != focus)
        {
            // was looking at an interactable, now is looking at different one
            focus = lookingAt;
            // redo to update the name
            EnableCrosshairEffects(focus.name);
        }

    }
    public void Interact()
    {
        focus?.OnInteract?.Invoke(this);
    }

    private void EnableCrosshairEffects(string info)
    {
        crosshairFocusEffect.gameObject.SetActive(true);
        crosshairInfoText.text = info;
        crosshairEffectsEnabled = true;
    }
    private void DisableCrosshairEffects()
    {
        crosshairFocusEffect.gameObject.SetActive(false);
        crosshairInfoText.text = "";
        crosshairEffectsEnabled = false;
    }
    private void OnEnable()
    {
        Start();
    }
    private void OnDisable()
    {
        DisableCrosshairEffects();
    }
}
