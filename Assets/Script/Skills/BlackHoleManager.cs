using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    [SerializeField] private GameObject hotKey;
    [SerializeField] private List<KeyCode> hotKeyList;

    public bool canGrow;
    public float growSpeed;
    public float maxSize;

    private List<Transform> targets = new List<Transform>();

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }
    private void CreateHotKey(Collider2D collision)
    {
        if(hotKeyList.Count <= 0)
        {
            Debug.Log("hotKey is 0)");
            return;
        }
        GameObject newHotKey = Instantiate(hotKey, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        KeyCode chooseKey = hotKeyList[Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(chooseKey);
        BlackHoleHotKey newBlackHoleHotKey = newHotKey.GetComponent<BlackHoleHotKey>();
        newBlackHoleHotKey.SetupHotKey(chooseKey, collision.transform, this);
    }
    public void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform); 
}
