using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private int explodeGravity;
    [SerializeField] private int explodeRange;

    [Header("Bounce Info")]
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private int amountOfFierce;
    [SerializeField] private float pierceGravity;

    [Header("Sword Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeDuration;
    [SerializeField] private float returnSpeed;

    private Vector2 finalDir;

    [Header("Dots Info")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private Transform dotParent;
    [SerializeField] private GameObject dotPrefab;

    private GameObject[] dots;
    protected override void Start()
    {
        base.Start();
        GenereateDots();
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
        newSwordManager.SetupSword(finalDir, swordGravity, player, freezeDuration, returnSpeed);
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
}
