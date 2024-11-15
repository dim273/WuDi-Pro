using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //单例模式
    public static Inventory instance;

    public List<ItemData> startingItem;

    //建立字典树，储存获取的物品，并且通过ItemData来查找，List用来储存物品的种类，数量，
    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;
    private float[] equipmentSkillTimer = { 0, 0, 0, 0 };   //武器技能冷却

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    //记录储存的物品在UI上的显示
    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    public void Start()
    {
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        AddStartingItem();
    }

    private void Update()
    {
        equipmentSkillTimer[0] -= Time.deltaTime;
        equipmentSkillTimer[1] -= Time.deltaTime;
        equipmentSkillTimer[2] -= Time.deltaTime;
        equipmentSkillTimer[3] -= Time.deltaTime;
    }

    private void AddStartingItem()
    {
        for(int i = 0; i < startingItem.Count; i++)
        {
            AddItem(startingItem[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }
        if(oldEquipment != null)
        {
            AddItem(oldEquipment);
            Unequipment(oldEquipment);
        }
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        RemoveItem(_item);
        newEquipment.AddModifiers();
        UpdateSlotUI();
    }

    public void Unequipment(ItemData_Equipment oldEquipment)
    {
        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem va))
        {
            oldEquipment.RemoveModifiers();
            //AddItem(oldEquipment);
            equipment.Remove(va);
            equipmentDictionary.Remove(oldEquipment);

        }
    }

    private void UpdateSlotUI()
    {
        //武器槽
        for(int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if(item.Key.equipmentType == equipmentSlot[i].equipmentType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        //更新UI的方法：先删除现有的UI的物品，然后在重新更新全部的物品
        for(int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for(int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }
        for(int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for(int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item)
    {
        //if (_item == null) return;
        //增加物品，武器放一栏，材料放一栏
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);
        else if(_item.itemType == ItemType.Material)
            AddToStash(_item);
        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
            value.AddStack();
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if(value.stackSize < 2)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }
        if(stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if(stashValue.stackSize < 2)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
                stashValue.RemoveStack();
        }
        UpdateSlotUI();
    }

    public List<InventoryItem> GetStashList() => stash;

    public List<InventoryItem> GetEquipmentList() => inventory;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }
        return equipedItem;
    }

    public void UseEquipmentSkillOnAttack(Transform _enemy)
    {
        ItemData_Equipment _Equipment = null;
        //攻击时遍历全部已装备的装备，装备技能为攻击类型的装备则释放技能
        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.skillType == SkillType.Attack)
            {
                _Equipment = item.Key;
                switch (_Equipment.equipmentType)
                {
                    case EquipmentType.Weapon:
                        if (equipmentSkillTimer[0] < 0)
                        {
                            _Equipment.ExcuteItemEffect(_enemy);
                            equipmentSkillTimer[0] = _Equipment.coolDown;
                        }
                        break;
                    case EquipmentType.Armor:
                        if (equipmentSkillTimer[1] < 0)
                        {
                            _Equipment.ExcuteItemEffect(_enemy);
                            equipmentSkillTimer[1] = _Equipment.coolDown;
                        }
                        break;
                    case EquipmentType.Ring:
                        if (equipmentSkillTimer[2] < 0)
                        {
                            _Equipment.ExcuteItemEffect(_enemy);
                            equipmentSkillTimer[2] = _Equipment.coolDown;
                        }
                        break;
                    case EquipmentType.Amulet:
                        if (equipmentSkillTimer[3] < 0)
                        {
                            _Equipment.ExcuteItemEffect(_enemy);
                            equipmentSkillTimer[3] = _Equipment.coolDown;
                        }
                        break;
                }
            }
        }
    }
}
