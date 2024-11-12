using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot -" + equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData equipmentData)
    {
        if (item == null) return;

        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        Inventory.instance.Unequipment(item.data as ItemData_Equipment);
        CleanUpSlot();
        
    }
}
