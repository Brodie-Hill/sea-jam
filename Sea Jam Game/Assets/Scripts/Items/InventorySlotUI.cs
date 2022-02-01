using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image sprite = null;
    [SerializeField] private TMP_Text countText = null;
    private Item item;

    public void AddItem(Item newItem)
    {
        item = newItem;
        
        sprite.sprite = item.GetSprite();
        sprite.enabled = true;
    }
    public void UpdateCount(int newCount)
    {
        countText.text = newCount.ToString();
    }

    public void ClearSlot()
    {
        item = null;
        sprite.sprite = null;
        sprite.enabled = false;
    }

    public Item GetItem() { return item; }

    public void SelectSlot()
    {
        GetComponent<Image>().color = GetComponent<Button>().colors.selectedColor;
    }
    public void DeselectSlot()
    {
        GetComponent<Image>().color = GetComponent<Button>().colors.normalColor;
    }
}
