using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemAmount;
    protected UI ui;

    public InventoryItem item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if(item != null)
        {
            itemImage.sprite = item.data.icon;
            if(item.stackSize > 1)
            {
                itemAmount.text = item.stackSize.ToString();
            }
            else
            {
                itemAmount.text = "";
            }
        }
    }
    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemAmount.text = "";
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemImage.sprite == null) return;
        if(item.data.itemType == ItemType.Equipment && item != null && item.data!)
            Inventory.instance.EquipItem(item.data);
        ui.itemTooltip.HideToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null || item.data == null) return;
        ui.itemTooltip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null || item.data == null) return;
        ui.itemTooltip.ShowToolTip(item.data as ItemData_Equipment);
       
    }
}
