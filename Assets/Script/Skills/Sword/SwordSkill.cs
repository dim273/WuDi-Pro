using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Explode
}
public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;


    [Header("Explode Info")]
    [SerializeField] private UI_SkillTreeSlot ballExplodeButton;
    [SerializeField] private int explodeGravity;
    [SerializeField] private int explodeRange;

    [Header("Bounce Info")]
    [SerializeField] private UI_SkillTreeSlot ballBounceButton;
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private UI_SkillTreeSlot ballPierceButton;
    [SerializeField] private int amountOfFierce;
    [SerializeField] private float pierceGravity;

    [Header("Sword Info")]
    [SerializeField] private UI_SkillTreeSlot ballButton;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeDuration;
    [SerializeField] private float returnSpeed;
    public bool unlockBallSkill { get; private set; }

    private Vector2 finalDir;

    [Header("Dots Info")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private Transform dotParent;
    [SerializeField] private GameObject dotPrefab;

    [Header("…À∫¶º”≥…")]
    [SerializeField] private UI_SkillTreeSlot damageAddOneButton;
    [SerializeField] private UI_SkillTreeSlot damageAddTwoButton;
    private int additionalDamage;

    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        GenereateDots();

        ballButton.GetComponent<Button>().onClick.AddListener(UnlockBallSkill);
        ballExplodeButton.GetComponent<Button>().onClick.AddListener(UnlockBallExplode);
        ballPierceButton.GetComponent<Button>().onClick.AddListener(UnlockBallPierce);
        ballBounceButton.GetComponent<Button>().onClick.AddListener(UnlockBallBounce);
        damageAddOneButton.GetComponent<Button>().onClick.AddListener(UnlockDamageAddOne);
        damageAddTwoButton.GetComponent<Button>().onClick.AddListener(UnlockDamageAddTwo);
    }

    protected override void Update()
    {
        base.Update();
        SetUpGravity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * launchDir.x, AimDirection().normalized.y * launchDir.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    private void SetUpGravity()
    {
        if(swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if(swordType ==  SwordType.Explode)
            swordGravity = explodeGravity;
    }

    public void CreateSword()
    {
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.y + 0.8f); 
        GameObject newSword = Instantiate(swordPrefab, position, transform.rotation);
        SwordManager newSwordManager = newSword.GetComponent<SwordManager>();

        if(swordType == SwordType.Bounce)
        {
            newSwordManager.SetupBounce(true, amountOfBounce, bounceSpeed);
        }
        else if(swordType== SwordType.Pierce)
        {
            newSwordManager.SetUpFierce(amountOfFierce);
        }
        else if(swordType == SwordType.Explode)
        {
            newSwordManager.SetupExplode(true, explodeRange);
        }
        newSwordManager.SetupSword(finalDir, swordGravity, player, freezeDuration, returnSpeed, additionalDamage);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }
    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchDir.x,
            AimDirection().normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;    
    }
    #endregion

    #region unlockSkill
    private void UnlockBallSkill()
    {
        if (ballButton.unlocked)
            unlockBallSkill = true;
    }

    private void UnlockBallExplode()
    {
        if(ballExplodeButton.unlocked)
            swordType = SwordType.Explode;
    }

    private void UnlockBallPierce()
    {
        if(ballPierceButton.unlocked)
            swordType = SwordType.Pierce;
    }

    private void UnlockBallBounce()
    {
        if(ballBounceButton.unlocked)
            swordType = SwordType.Bounce;
    }
    private void UnlockDamageAddOne()
    {
        if (damageAddOneButton.unlocked)
            additionalDamage += 10;
    }
    private void UnlockDamageAddTwo()
    {
        if (damageAddTwoButton.unlocked)
            additionalDamage += 30;
    }
    #endregion
}
