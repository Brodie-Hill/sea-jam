using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public event Action onUpdatedCallback;

    private Item[] items;
    private int[] counts;

    [SerializeField] int capacity = 20;

    private void Start()
    {
        items = new Item[capacity];
        counts = new int[capacity];
    }
    
    public bool Add(Item item)
    {
        int freeSpace = NextFreeSpace(item);

        if (freeSpace == -1)
        {
            Debug.Log("Not enough space to add item");
            return false;
        }

        items[freeSpace] = item;
        counts[freeSpace]++; // items only added 1 at a time for now

        onUpdatedCallback?.Invoke();

        Debug.Log("Added item " + item.name);

        return true;
    }

    public void Remove(Item item)
    {
            Remove(IndexOf(item));
    }
    public void Remove(int index)
    {
        if (PeekItem(index) != null)
        {
            if (--counts[index] == 0)
                items[index] = null;
        }
            
        onUpdatedCallback?.Invoke();
    }

    public void Drop(int index)
    {
        if (items[index] == null) return;

        Instantiate(items[index].GetDrop(), transform.position, Quaternion.identity);

        Remove(index);
        onUpdatedCallback?.Invoke();
    }


    private int NextFreeSpace(Item item)
    {
        for (int i = 0; i < capacity; i++)
        {
            // if there is a free space
            if (items[i] == null) return i;
            // or if there is a stack of these items that is not full
            if (items[i] == item && counts[i] < item.GetStackSize()) return i;
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
    public Item PeekItem(int index)
    {
        if (index < 0 || index >= capacity) return null;
        return items[index];
    }
    public int PeekCount(int index)
    {
        if (index < 0 || index >= capacity) return -1;
        return counts[index];
    }
    public int Capacity()
    {
        return capacity;
    }
}
