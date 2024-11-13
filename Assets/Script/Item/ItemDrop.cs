using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDrop : MonoBehaviour
{
    //������Ʒ�嵥���Լ���Ӧ��Ʒ�ĵ�����ʣ�����ɵ͵��߱�ʾ��Ʒ��ϡ�ȣ�Խ�ߵ�����Խ��
    [SerializeField] private ItemData[] possibleDrop;   
    [SerializeField] private int[] dropChance;
    public int doubleDropChance;    //����ÿ����Ʒʱ��˫��������
    
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;
    //[SerializeField] private ItemData item;

    public void GenerateDrop()
    {
        //�ж���Ʒ�Ƿ����
        for(int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0, 100) <= dropChance[i])
            {
                dropList.Add(possibleDrop[i]);
                if(Random.Range(0, 100) <= doubleDropChance)
                    dropList.Add(possibleDrop[i]);
            }
        }
        DropItem(possibleDrop[0]);      //���׵�����ͼ���Ʒ
        if (possibleDrop.Length <= 0) return;
        for(int i = 0; i < possibleDrop.Length - 1; i++)
        {
            DropItem(possibleDrop[i]);
        }
    }

    public void DropItem(ItemData _itemData)
    {
        //���ɵ�����Ʒ
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-3, 4), Random.Range(12, 15));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
