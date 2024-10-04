using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float cloneAttackCoolDown;
    [SerializeField] private float blackHoleDuration;

    BlackHoleManager newBlackHoleManager;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        newBlackHoleManager = newBlackHole.GetComponent<BlackHoleManager>();
        newBlackHoleManager.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, cloneAttackCoolDown, blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    public bool SkillCompleted()
    {
        if (newBlackHoleManager == null)
            return false;
        if (newBlackHoleManager.playerCanExitState)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
