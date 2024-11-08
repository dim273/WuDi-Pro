using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    public void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
    }
    public void AddItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if(value.stackSize < 2)
            {
                inventoryDictionary.Remove(_item);
                inventoryItems.Remove(value);
            }
            else
                value.RemoveStack();
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            ItemData newItem = inventoryItems[inventoryItems.Count - 1].data;
            RemoveItem(newItem);
        }
    }
}
