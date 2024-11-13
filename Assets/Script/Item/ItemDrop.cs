using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDrop : MonoBehaviour
{
    //掉落物品清单，以及对应物品的掉落概率，序号由低到高表示物品珍稀度，越高掉落率越低
    [SerializeField] private ItemData[] possibleDrop;   
    [SerializeField] private int[] dropChance;
    public int doubleDropChance;    //掉落每件物品时的双倍掉落率
    
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;
    //[SerializeField] private ItemData item;

    public void GenerateDrop()
    {
        //判断物品是否掉落
        for(int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0, 100) <= dropChance[i])
            {
                dropList.Add(possibleDrop[i]);
                if(Random.Range(0, 100) <= doubleDropChance)
                    dropList.Add(possibleDrop[i]);
            }
        }
        DropItem(possibleDrop[0]);      //保底掉落最低级物品
        if (possibleDrop.Length <= 0) return;
        for(int i = 0; i < possibleDrop.Length - 1; i++)
        {
            DropItem(possibleDrop[i]);
        }
    }

    public void DropItem(ItemData _itemData)
    {
        //生成掉落物品
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-3, 4), Random.Range(12, 15));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
