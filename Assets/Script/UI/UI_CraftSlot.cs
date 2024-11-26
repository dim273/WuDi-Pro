using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
        SetUpCraftSlot(item.data as ItemData_Equipment);
    }

    public void SetUpCraftSlot(ItemData_Equipment _item)
    {
        if (item == null) return;
        item.data = _item;
        itemImage.sprite = _item.icon;
        itemText.text = _item.itemName;
    }

    private void OnValidate()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
