using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name = null;
    [SerializeField]
    private Sprite sprite = null;
    [SerializeField]
    private GameObject droppedModel = null;
    [SerializeField]
    private int stackSize = 99;

    public Sprite GetSprite()
    {
        return sprite;
    }
    public GameObject GetDrop()
    {
        return droppedModel;
    }
    public int GetStackSize()
    {
        return stackSize;
    }
}
