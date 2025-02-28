using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_ItemTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;

    public void ShowToolTip(ItemData_Equipment item)
    {
        if (item == null) return;

        itemNameText.text = item.itemName;

        switch (item.equipmentType)
        {
            case EquipmentType.Weapon: itemTypeText.text = "武器"; break;
            case EquipmentType.Armor: itemTypeText.text = "护甲"; break;
            case EquipmentType.Ring: itemTypeText.text = "指环"; break;
            case EquipmentType.Amulet: itemTypeText.text = "消耗品"; break;
        }

        itemDescription.text = item.GetDescription();
        AdjustFontSize(itemNameText);
        AdjustPosition();
        gameObject.SetActive(true);
        
    }

    public void HideToolTip()
    {
        //关闭组件
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
