using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("��ɫ����")]
    [SerializeField] private float chanceToLoseItem;
    public override void GenerateDrop()
    {
        //��ɫ����������Ʒ
        Inventory inventory = Inventory.instance;

        List<InventoryItem> ItemToLose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetStashList())
        {
            if(Random.Range(0, 100) <= chanceToLoseItem)
            {
                DropItem(item.data);
                ItemToLose.Add(item);
            }
        }
        for(int i = 0; i < ItemToLose.Count; i++)
        {
            inventory.RemoveItem(ItemToLose[i].data);
        }
    }
}
