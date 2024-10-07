using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSkill : Skill
{
    [SerializeField] private float chickenDuration;
    [SerializeField] private GameObject chickenPrefab;
    private GameObject chickenGameobject;
    private float defaultCoolDown;

    [Header("Explode Chicken")]
    [SerializeField] private bool canExplode;

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove;

    [Header("Multi Stacking Chicken")]
    [SerializeField] private bool canMulti;
    [SerializeField] private int amountOfChicken;
    [SerializeField] float multiCoolDown;
    [SerializeField] List<GameObject> chickenLeft = new List<GameObject>();
    [SerializeField] private float useTimeWindow;

    [Header("Create In Dash State")]
    [SerializeField] private bool canCreateInDashState;

    public override void UseSkill()
    {
        base.UseSkill();
        if (CanUseMultiChicken())
            return;
        if(chickenGameobject == null && !canCreateInDashState)
        {
            chickenGameobject = Instantiate(chickenPrefab, player.transform.position, Quaternion.identity);
            ChickenManager newChickenManager = chickenGameobject.GetComponent<ChickenManager>();
            newChickenManager.SetupChicken(chickenDuration, canExplode, canMove, moveSpeed, FindClonestEnemy(chickenGameobject.transform));
            if (!canMove)
            {
                coolDown = 0;
                StartCoroutine("CoolDownRecover", chickenDuration);
            }
        }
        else
        {
            if (canMove)
                return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = chickenGameobject.transform.position;
            chickenGameobject.transform.position = playerPos;
            if(!canCreateInDashState)
                coolDown = defaultCoolDown;
            chickenGameobject.GetComponent<ChickenManager>().FinishChicken();
        }
    }
    private bool CanUseMultiChicken()
    {
        if (canMulti)
        {
            if(chickenLeft.Count > 0 && coolDownTimer < 0)
            {
                if(chickenLeft.Count == amountOfChicken)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }
                coolDown = 0;
                GameObject chickenToSpawn = chickenLeft[chickenLeft.Count - 1];
                GameObject newChicken = Instantiate(chickenToSpawn, player.transform.position, Quaternion.identity);
                chickenLeft.Remove(chickenToSpawn);
                newChicken.GetComponent<ChickenManager>().SetupChicken(
                    chickenDuration, canExplode, canMove, moveSpeed, FindClonestEnemy(newChicken.transform));
                if(chickenLeft.Count <= 0)
                {
                    coolDown = multiCoolDown;
                    RefilChicken();
                }
            }
            return true;
        }
        return false;
    }
    private void RefilChicken()
    {
        int amountToAdd = amountOfChicken - chickenLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            chickenLeft.Add(chickenPrefab);
        }
    }
    private void ResetAbility()
    {
        if (coolDownTimer > 0)
            return;
        coolDown = multiCoolDown;
        RefilChicken();
    }
    public void CreateChicken()
    {
        if (canCreateInDashState)
        {
            coolDown = 0;
            if (chickenGameobject != null)
                Destroy(chickenGameobject);
            chickenGameobject = Instantiate(chickenPrefab, player.transform.position, Quaternion.identity);
            ChickenManager newChickenManager = chickenGameobject.GetComponent<ChickenManager>();
            newChickenManager.SetupChicken(chickenDuration, canExplode, canMove, moveSpeed, FindClonestEnemy(chickenGameobject.transform));
            StartCoroutine("CreateChickenCD", defaultCoolDown);
        }
    }
    private IEnumerator CreateChickenCD(int _time)
    {
        canCreateInDashState = false;
        yield return new WaitForSeconds(_time);
        canCreateInDashState = true;
    }
    private IEnumerator CoolDownRecover(int _time)
    {
        yield return new WaitForSeconds(_time);
        coolDown = defaultCoolDown;
    }
    protected override void Start()
    {
        base.Start();
        defaultCoolDown = coolDown;
    }
}
