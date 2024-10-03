using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    [SerializeField] private GameObject hotKey;
    [SerializeField] private List<KeyCode> hotKeyList;

    public bool canGrow;
    public float growSpeed;
    public bool canShrink;
    public float shrinkSpeed;
    public float maxSize;

    public bool canCreateHotKey = true;
    private bool cloneAttackRelease;
    private int attackIndex = 0;
    public float cloneAttackCoolDown = .3f;
    private float cloneAttackTimer;

    private List<GameObject> createdHotKey = new List<GameObject>();
    private List<Transform> targets = new List<Transform>();

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        Debug.Log(targets.Count);
        Debug.Log(" attackIdex : " + attackIndex);
        if (Input.GetKeyDown(KeyCode.M))
        {
            cloneAttackRelease = true; 
            canCreateHotKey = false;
            attackIndex = targets.Count - 1;
            DestroyHotKey();
        }
        if(cloneAttackTimer < 0 && cloneAttackRelease)
        {
            cloneAttackTimer = cloneAttackCoolDown;
            float _offset;
            if (Random.Range(0, 100) > 50)
                _offset = 2;
            else
                _offset = -2;
            SkillManager.instance.clone.CreateClone(targets[attackIndex], new Vector3(_offset, 0, 0));
            attackIndex--;
            if(attackIndex < 0)
            {
                cloneAttackRelease = false;
                canShrink = true;
            }
        }
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime); 
            if(transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
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
        if(hotKeyList.Count == 0)
        {
            return;
        }
        if (!canCreateHotKey)
        {
            return;
        }
        GameObject newHotKey = Instantiate(hotKey, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);
        KeyCode chooseKey = hotKeyList[Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(chooseKey);
        BlackHoleHotKey newBlackHoleHotKey = newHotKey.GetComponent<BlackHoleHotKey>();
        newBlackHoleHotKey.SetupHotKey(chooseKey, collision.transform, this);
    }
    private void DestroyHotKey()
    {
        if(createdHotKey.Count <= 0)
        {
            return;
        }
        for(int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }
    public void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform); 
}
