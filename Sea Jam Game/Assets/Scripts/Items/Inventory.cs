using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public event Action onUpdatedCallback;

    private Item[] items;
    [SerializeField] int capacity = 20;

    private void Start()
    {
        items = new Item[capacity];
    }
    public bool Add(Item item)
    {
        int freeSpace = NextFreeSpace();

        if (freeSpace == -1)
        {
            Debug.Log("Not enough space to add item");
            return false;
        }

        items[freeSpace] = item;

        onUpdatedCallback?.Invoke();

        Debug.Log("Added item " + item.name);

        return true;
    }

    public void Remove(Item item)
    {
        int index = IndexOf(item);
        if (index != -1)
            items[index] = null;
        onUpdatedCallback?.Invoke();
    }
    public void Remove(int index)
    {
        if (Peek(index) != null)
            items[index] = null;
        onUpdatedCallback?.Invoke();
    }

    public void Drop(int index)
    {
        if (items[index] == null) return;

        Instantiate(items[index].GetDrop(), transform.position, Quaternion.identity);

        Remove(index);
        onUpdatedCallback?.Invoke();
    }


    private int NextFreeSpace()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (items[i] == null) return i;
        }
        return -1;
    }
    private int IndexOf(Item item)
    {
        for (int i = 0; i < capacity; i++)
        {
            if (items[i] == item) return i;
        }
        return -1;
    }


    // accessability methods
    public Item Peek(int index)
    {
        if (index < 0 || index >= capacity) return null;
        return items[index];
    }
    public int Capacity()
    {
        return capacity;
    }
}
