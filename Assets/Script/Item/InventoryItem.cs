using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//建立一个封装类
[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;
    public InventoryItem(ItemData _newData)
    {
        data = _newData;
        AddStack();
    }
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
