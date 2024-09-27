using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefah;
    [SerializeField] private float cloneDuration;
    public void CreateClone(Transform _cloneTransform)
    {
        GameObject newClone = Instantiate(clonePrefah);
        newClone.GetComponent<CloneManager>().SetupClone(_cloneTransform, cloneDuration);
    }
}
