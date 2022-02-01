using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private Inventory inventory = null;
    [SerializeField] private Transform slotsParent = null;
    [SerializeField] private Text itemNameText = null;
    private InventorySlotUI[] slots;

    private InventorySlotUI selected;

    private void Start()
    {
        slots = slotsParent.GetComponentsInChildren<InventorySlotUI>();
        if (slots.Length != inventory.Capacity())
        {
            Debug.LogWarning("Amount of slots in inventory UI doesn't match the internal inventory capacity");
        }

        foreach (InventorySlotUI slotUI in slots)
        {
            slotUI.GetComponent<Button>().onClick.AddListener(delegate { Select(slotUI); });
        }
        
        OnEnable();
    }
    private void OnEnable()
    {
        itemNameText.text = "";
        selected = null;
        
        inventory.onUpdatedCallback += UpdateUI;
        // items that were added while not open wont show so update again
        UpdateUI();
    }
    private void OnDisable()
    {
        inventory.onUpdatedCallback -= UpdateUI;
    }

    private void UpdateUI()
    {
        Item item;
        int count;
        for (int i = 0; i < slots.Length; i++)
        {
            item = inventory.PeekItem(i);
            count = inventory.PeekCount(i);
            if (item == null) {
                slots[i].ClearSlot();
            }
            else
            {
                slots[i].AddItem(item);
            }
            slots[i].UpdateCount(count);
        }
    }

    public void Select(InventorySlotUI slot)
    {
        selected?.DeselectSlot();
        selected = slot;
        selected.SelectSlot();
        Item i;
        itemNameText.text = ((i = slot.GetItem()) != null) ? i.name : "";
    }

    public void DropItem()
    {
        if (selected == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i]==selected)
            {
                inventory.Drop(i);
            }
        }

    }
    public void TrashItem()
    {
        if (selected == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == selected)
            {
                inventory.Remove(i);
            }
        }

    }
}
