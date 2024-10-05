using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSkill : Skill
{
    [SerializeField] private float chickenDuration;
    [SerializeField] private GameObject chickenPrefab;
    private GameObject chickenGameobject;

    [Header("Explode Chicken")]
    [SerializeField] private bool canExplode;

    public override void UseSkill()
    {
        base.UseSkill();
        if(chickenGameobject == null)
        {
            chickenGameobject = Instantiate(chickenPrefab, player.transform.position, Quaternion.identity);
            ChickenManager newChickenManager = chickenGameobject.GetComponent<ChickenManager>();
            newChickenManager.SetupChicken(chickenDuration, canExplode);
        }
        else
        {
            Vector2 playerPos = player.transform.position;
            player.transform.position = chickenGameobject.transform.position;
            chickenGameobject.transform.position = playerPos;
            chickenGameobject.GetComponent<ChickenManager>().FinishChicken();
        }
    }
}
