using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;

    public InventoryItem item;

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
    }
}
