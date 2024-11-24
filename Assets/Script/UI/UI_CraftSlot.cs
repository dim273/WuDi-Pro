using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetUpCraftSlot(ItemData_Equipment _item)
    {
        item.data = _item;
        itemImage.sprite = _item.icon;
        itemText.text = _item.name;
    }

    private void OnValidate()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;
        Inventory.instance.CanCraft(craftData, craftData.craftinggMaterials);
    }
}
