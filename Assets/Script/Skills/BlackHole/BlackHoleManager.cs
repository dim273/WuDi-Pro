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
    private float blackHoleTimer;

    private bool canCreateHotKey = true;
    private bool cloneAttackRelease = false;
    private bool playerCanDisappear = true;

    private int attackIndex = 6;
    private float cloneAttackCoolDown;
    private float cloneAttackTimer;

    private List<GameObject> createdHotKey = new List<GameObject>();
    private List<Transform> targets = new List<Transform>();

    public bool playerCanExitState {  get; private set; }

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, float _coolDown, float _blackHoleTimer)
    {
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        maxSize = _maxSize;
        cloneAttackCoolDown = _coolDown;
        blackHoleTimer = _blackHoleTimer;
    }
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer <= 0)
        {
            blackHoleTimer = Mathf.Infinity;
            if (targets.Count > 0)
            {
                ReleaseSkill();
            }
            else

                FinishBlackholeAbility();
        }
        if (Input.GetKeyDown(KeyCode.M) && targets.Count > 0)
        {
            ReleaseSkill();
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
    private void ReleaseSkill()
    {
        if (targets.Count <= 0)
            return;

        cloneAttackRelease = true;
        canCreateHotKey = false;
        attackIndex = 6;
        DestroyHotKey();
        if (playerCanDisappear)
        {
            PlayerManager.instance.player.MakeTransprent(true);
            playerCanDisappear = false;
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
        DestroyHotKey();
        cloneAttackRelease = false;
        canShrink = true;
        playerCanExitState = true;
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
