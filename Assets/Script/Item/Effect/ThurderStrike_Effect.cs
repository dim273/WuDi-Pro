using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "Thurder strike effect", menuName = "Data/Item effect/Thurder strike")]
public class ThurderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thurderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThurderStrike = Instantiate(thurderStrikePrefab, _enemyPosition.position, Quaternion.identity);
        Destroy(newThurderStrike, 1f);
    }
}
