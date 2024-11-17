using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    armor,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}
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
        stat.AddBuff(buffAmount, buffDuration, StatToModify());
    }

    private Stat StatToModify()
    {
        //找到对应的buff种类
        switch (type)
        {
            case StatType.strength: return stat.strength;
            case StatType.agility: return stat.agility;
            case StatType.intelligence: return stat.intellgence;
            case StatType.vitality: return stat.vitality;
            case StatType.damage: return stat.damage;
            case StatType.critChance: return stat.critChance;
            case StatType.critPower: return stat.critPower;
            case StatType.armor: return stat.armor;
            case StatType.magicResistance: return stat.magicResisitance;
            case StatType.fireDamage: return stat.fireDamage;
            case StatType.iceDamage: return stat.iceDamage;
            case StatType.lightingDamage: return stat.lightingDamage;
        }
        return null;
    }
}
