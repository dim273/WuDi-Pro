using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("¿§…¡")]
    public bool cloneDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneDashButton;

    [Header("¿§ŒÚ")]
    public bool duplicateClone;
    [SerializeField] private UI_SkillTreeSlot duplicateCloneButton;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();
        cloneDashButton.GetComponent<Button>().onClick.AddListener(UnlockClone);
        duplicateCloneButton.GetComponent<Button>().onClick.AddListener(UnlockDeplicateClone);
    }

    private void UnlockClone()
    {
        if (cloneDashButton.unlocked)
            cloneDashUnlocked = true;
    }

    private void UnlockDeplicateClone()
    {
        if(duplicateCloneButton.unlocked)
            duplicateClone = true;
        SkillManager.instance.clone.CanDuplicateClone(duplicateClone);
    }

    public void CloneOnDash()
    {
        base.UseSkill();
        if(cloneDashUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
    }
}
