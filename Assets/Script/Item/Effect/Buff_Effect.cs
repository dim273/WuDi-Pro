using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class Buff_Effect : ItemEffect
{
    private PlayerStat stat;
    [SerializeField] private StatType type;             //buff����
    [SerializeField] private int buffAmount;            //���ӵ���ֵ
    [SerializeField] private float buffDuration;        //����ʱ��

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        stat.AddBuff(buffAmount, buffDuration, stat.GetStats(type));
    }
}
