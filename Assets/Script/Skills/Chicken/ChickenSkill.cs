using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChickenSkill : Skill
{
    [SerializeField] private float chickenDuration;
    [SerializeField] private GameObject chickenPrefab;
    private GameObject chickenGameobject;
    private float defaultCoolDown;

    [Header("Ð¡¼¦")]
    [SerializeField] private UI_SkillTreeSlot createChickenButton;
    public bool chickenSkillUnlocked {  get; private set; }

    [Header("Ð¡¼¦±¬Õ¨")]
    [SerializeField] private bool canExplode;
    [SerializeField] private UI_SkillTreeSlot explodeButton;

    [Header("Ð¡¼¦×·×Ù")]
    [SerializeField] private UI_SkillTreeSlot canMoveButton;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool canMove;

    [Header("Multi Stacking Chicken")]
    [SerializeField] private bool canMulti;
    [SerializeField] private int amountOfChicken;
    [SerializeField] float multiCoolDown;
    [SerializeField] List<GameObject> chickenLeft = new List<GameObject>();
    [SerializeField] private float useTimeWindow;


    public override void UseSkill()
    {
        base.UseSkill();
        if (CanUseMultiChicken())
            return;
        if(chickenGameobject == null)
        {
            CreateChicken();
        }
        else
        {
            if (canMove)
                return;
            Vector2 playerPos = player.transform.position;
            player.transform.position = chickenGameobject.transform.position;
            chickenGameobject.transform.position = playerPos;
            //coolDown = defaultCoolDown;
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
                    chickenDuration, canExplode, canMove, moveSpeed, FindClonestEnemy(newChicken.transform),player);
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

    public void CurrentChickenChooseRandomTarget() => chickenGameobject.GetComponent<ChickenManager>().ChooseRandomEnemy();
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
        chickenGameobject = Instantiate(chickenPrefab, player.transform.position, Quaternion.identity);
        ChickenManager newChickenManager = chickenGameobject.GetComponent<ChickenManager>();
        newChickenManager.SetupChicken(chickenDuration, canExplode, canMove, moveSpeed, FindClonestEnemy(chickenGameobject.transform), player);
    }

    protected override void Start()
    {
        base.Start();
        defaultCoolDown = coolDown;

        createChickenButton.GetComponent<Button>().onClick.AddListener(UnlockChicken);
        explodeButton.GetComponent<Button>().onClick.AddListener(UnlockChickenExplode);
        canMoveButton.GetComponent<Button>().onClick.AddListener(UnlockChickenMove);
    }

    #region unlockSkill
    private void UnlockChicken()
    {
        if (createChickenButton.unlocked)
            chickenSkillUnlocked = true;
    }

    private void UnlockChickenExplode()
    {
        if(explodeButton.unlocked)
            canExplode = true;
    }

    private void UnlockChickenMove()
    {
        if (canMoveButton.unlocked)
        {
            canMove = true;
            canMulti = true;
        }
    }
    #endregion
}
