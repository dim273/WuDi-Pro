using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefah;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    public void CreateClone(Transform _cloneTransform, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefah);
        newClone.GetComponent<CloneManager>().SetupClone(_cloneTransform, cloneDuration, canAttack, _offset, FindClonestEnemy(newClone.transform));
    }
}
