using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemy effect", menuName = "Data/Item effect/Freeze enemy effect")]
public class FreezeEnemy_Effect : ItemEffect
{
    [SerializeField] private float freezeDuration;

    public override void ExecuteEffect(Transform _myPosition)
    {
        //使用条件
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        if (playerStat.currentHealth > playerStat.GetMaxHealthValue() * 0.2f)
            return;
        if (!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_myPosition.position, 2f);
        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>()?.FreezeStart(freezeDuration);
        }

    }
}
