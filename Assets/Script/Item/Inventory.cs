using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;


public class Inventory : MonoBehaviour, ISaveManager
{
    //单例模式
    public static Inventory instance;

    public List<ItemData> startingItem;

    //建立字典树，储存获取的物品，并且通过ItemData来查找，List用来储存物品的种类，数量，
    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    //记录储存的物品在UI上的显示
    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("装备冷却")]
    public float[] equipmentSkillTimer = { 0, 0, 0, 0 };   //武器技能冷却
    private float armorSkillUsedTime;                //上次使用装备冷却的时间

    [Header("数据库")]
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;

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
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

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
        foreach(ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if(loadedItems.Count > 0)   //载入文件存在时调用
        {
            foreach(InventoryItem item in loadedItems)
            {
                Debug.Log("wew");
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }
        for(int i = 0; i < startingItem.Count; i++)
        {
            if(startingItem[i] != null)
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
            UpdateSlotUI();
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
        for(int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
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

    public bool CanUseArmor()
    {
        //检测是否可以使用装备技能
        ItemData_Equipment _armor = GetEquipment(EquipmentType.Armor);
        if (_armor == null) return false;

        if(Time.time > armorSkillUsedTime + _armor.coolDown)
        {
            armorSkillUsedTime = Time.time;
            return true;
        }
        return false;
    }

    public bool CanAddItem()
    {
        //检测是否可以向背包加入物品
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("No Space");
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiedMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for(int i = 0; i< _requiedMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiedMaterials[i].data, out InventoryItem stashValue))
            {
                if(stashValue.stackSize < _requiedMaterials[i].stackSize)
                {
                    Debug.Log("材料不够");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                return false;
            }
        }
        for(int i = 0; i < materialsToRemove.Count; i++)
            RemoveItem(materialsToRemove[i].data);

        AddItem(_itemToCraft);
        return true;
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }
        foreach(string equipmentID in _data.equipmentID)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemID == equipmentID)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();

        foreach(KeyValuePair<ItemData,InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentID.Add(pair.Key.itemID);
        }
    }

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Item" });
        foreach(string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
}
