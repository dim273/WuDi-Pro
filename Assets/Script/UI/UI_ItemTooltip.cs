using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
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

        //���ñ��������С�������۸�
        if(itemNameText.text.Length > 12)
        {
            itemNameText.fontSize = defaultFontSize * .7f;
        }
        else
        {
            itemNameText.fontSize = defaultFontSize;
        }
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        //�ر����
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
