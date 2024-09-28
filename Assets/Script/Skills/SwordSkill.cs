using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Sword Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;

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
    public void CreateSword()
    {
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.y + 0.8f); 
        GameObject newSword = Instantiate(swordPrefab, position, transform.rotation);
        SwordManager newSwordManager = newSword.GetComponent<SwordManager>();
        newSwordManager.SetupSword(finalDir, swordGravity, player);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }
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
}
