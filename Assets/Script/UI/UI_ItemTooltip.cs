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

        itemNameText.text = item.name;
        itemTypeText.text = item.equipmentType.ToString();
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
