using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefah;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    //[Header("Create Pos")]
    //[SerializeField] private bool canCreateCloneOnStart;

    [Header("Chicken Instead Clone")]
    public bool chooseChickenInsteadClone;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    public void CanDuplicateClone(bool _can)
    {
        canDuplicateClone = _can;
    }
    public void CreateClone(Transform _cloneTransform, Vector3 _offset)
    {
        
        if (chooseChickenInsteadClone)
        {
            SkillManager.instance.chicken.CreateChicken();
            return;
        }
        GameObject newClone = Instantiate(clonePrefah);
        newClone.GetComponent<CloneManager>().SetupClone(_cloneTransform, cloneDuration, canAttack, _offset, 
            FindClonestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate, player);
    }
}