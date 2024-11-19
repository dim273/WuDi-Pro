using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + equipmentType.ToString();
    }

    public override void OnPointerDown(PointerEventData equipmentData)
    {
        if (item == null || item.data == null) return;       //避免点击空装备槽出现报错

        Inventory.instance.AddItem(item.data as ItemData_Equipment);        //将装备重新放回装备栏
        Inventory.instance.Unequipment(item.data as ItemData_Equipment);    //从装备槽卸下装备
        CleanUpSlot();
        
    }
}
