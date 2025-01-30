using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{
    public bool unlockBlackHole { get; private set; }

    [SerializeField] private UI_SkillTreeSlot canUseBlackHoleSkillButton;
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
        canUseBlackHoleSkillButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHoleSkill);
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

    protected override void CheckUnlock()
    {
        UnlockBlackHoleSkill();
    }
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

    private void UnlockBlackHoleSkill()
    {
        if (canUseBlackHoleSkillButton.unlocked)
            unlockBlackHole = true;
    }
}
