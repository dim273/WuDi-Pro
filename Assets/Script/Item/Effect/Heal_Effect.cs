using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();

        int healAmount = Mathf.RoundToInt(playerStat.GetMaxHealthValue() * healPercent);
        if(healAmount <= 0)
            healAmount = 1;

        playerStat.IncreaseHealth(healAmount);
    }
}
