using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefah;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [SerializeField] private bool canCreateCloneOnStart;

    [SerializeField] private bool canCreateCloneOnOver;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;
    public void CreateClone(Transform _cloneTransform, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefah);
        newClone.GetComponent<CloneManager>().SetupClone(_cloneTransform, cloneDuration, canAttack, _offset, FindClonestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate);
    }
    public void CreateCloneOnStart()
    {
        if (canCreateCloneOnStart)
            CreateClone(player.transform, Vector3.zero);
    }
    public void CreateCloneOnOver()
    {
        if (canCreateCloneOnOver)
            CreateClone(player.transform, Vector3.zero);
    }
}