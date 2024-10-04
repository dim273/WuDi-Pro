using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleManager : MonoBehaviour
{
    [SerializeField] private GameObject hotKey;
    [SerializeField] private List<KeyCode> hotKeyList;

    private bool canGrow = true;
    private float growSpeed;
    private bool canShrink;
    private float shrinkSpeed;
    private float maxSize;

    private bool canCreateHotKey = true;
    private bool cloneAttackRelease = false;
    private int attackIndex = 4;
    private float cloneAttackCoolDown;
    private float cloneAttackTimer;

    private List<GameObject> createdHotKey = new List<GameObject>();
    private List<Transform> targets = new List<Transform>();

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _coolDown)
    {
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        maxSize = _maxSize;
        cloneAttackCoolDown = _coolDown;
    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.M))
        {
            cloneAttackRelease = true;
            canCreateHotKey = false;
            attackIndex = 4;
            DestroyHotKey();
            PlayerManager.instance.player.MakeTransprent(true);
        }
        CloneAttack();
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void CloneAttack()
    {
        if (cloneAttackTimer < 0 && cloneAttackRelease)
        {
            cloneAttackTimer = cloneAttackCoolDown;
            float _offset;
            if (Random.Range(0, 100) > 50)
                _offset = 1;
            else
                _offset = -1;
            int cut = Random.Range(0, targets.Count);
            SkillManager.instance.clone.CreateClone(targets[cut], new Vector3(_offset, 0, 0));
            attackIndex--;
            if (attackIndex <= 0)
            {
                Invoke("FinishBlackholeAbility", 0.5f);
            }
        }
    }
    private void FinishBlackholeAbility()
    {
        cloneAttackRelease = false;
        canShrink = true;
        PlayerManager.instance.player.ExitBlackholeAbility();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(false);
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
