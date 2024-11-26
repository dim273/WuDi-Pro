using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlot;

    [SerializeField] private List<ItemData_Equipment> craftEquipment;
    [SerializeField] private List<UI_CraftSlot> craftSlots;

    void Start()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UI_CraftSlot>());
        }
    }

    public void SetupCraftList()
    {
        for(int i = 0; i < craftSlots.Count; i++)
        {
            Destroy(craftSlots[i].gameObject);
        }
        craftSlots = new List<UI_CraftSlot>();
        for(int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlot, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipment[i]);
            craftSlots.Add(newSlot.GetComponent<UI_CraftSlot>());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }
}