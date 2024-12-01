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

        //控制提示框的位置
        Vector2 mousePos = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePos.x > 600)
            xOffset = -150;
        else
            xOffset = 150;

        if (mousePos.y > 320)
            yOffset = -150;
        else
            yOffset = 150;

        ui.itemTooltip.ShowToolTip(item.data as ItemData_Equipment);
        ui.itemTooltip.transform.position = new Vector2(mousePos.x + xOffset, yOffset + mousePos.y);
    }
}
