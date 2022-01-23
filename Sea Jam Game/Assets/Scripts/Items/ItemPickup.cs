using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : Interactable
{
    [SerializeField] private Item item = null;

    private void Awake()
    {
        name = item.name;
        OnInteract.AddListener(Pickup);
    }

    void Pickup(Interactor player)
    {
        Debug.Log("Picked Up Item");

        // add to inventory
        if (player.gameObject.TryGetComponent(out Inventory inv))
        {
            if (inv.Add(item)) Destroy(gameObject);
        }

        
    }
}
