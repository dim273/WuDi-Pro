using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStat stat;
    [SerializeField] private StatType type;             //buff类型
    [SerializeField] private int buffAmount;            //增加的数值
    [SerializeField] private float buffDuration;        //持续时间

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        stat.AddBuff(buffAmount, buffDuration, stat.GetStats(type));
    }
}
