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
            case EquipmentType.Weapon: itemTypeText.text = "����"; break;
            case EquipmentType.Armor: itemTypeText.text = "����"; break;
            case EquipmentType.Ring: itemTypeText.text = "ָ��"; break;
            case EquipmentType.Amulet: itemTypeText.text = "����Ʒ"; break;
        }

        itemDescription.text = item.GetDescription();
        AdjustFontSize(itemNameText);
        AdjustPosition();
        gameObject.SetActive(true);
        
    }

    public void HideToolTip()
    {
        //�ر����
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
