using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("¿§…¡")]
    [SerializeField] private UI_SkillTreeSlot cloneDashButton;
    public bool cloneDashUnlocked {  get; private set; }

    [Header("¿§ŒÚ")]
    [SerializeField] private UI_SkillTreeSlot duplicateCloneButton;
    public bool duplicateClone { get; private set; }
    

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

    protected override void CheckUnlock()
    {
        UnlockClone();
        UnlockDeplicateClone();
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
