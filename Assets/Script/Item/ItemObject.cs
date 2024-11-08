using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemData itemData;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.icon;
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()  != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
