using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strenth;      //力量，每一点可以提供1点物理伤害
    public Stat agility;      //敏捷，每一点可以提供1%攻速加成(40%为上限)和1%速度加成(20%为上限)
    public Stat intelgence;   //智慧，每一点可以提供1魔法伤害
    public Stat vitality;     //体力，每一点可以提供生命加成

    [Header("Defensive stats")]
    public Stat maxHealth;      //最大生命
    public Stat armor;          //护甲值，每一点提供1伤害减免

    public Stat damage;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totleDamage = damage.GetValue() + strenth.GetValue();
        totleDamage = CheckTargetArmor(_targetStats, totleDamage);
        _targetStats.TakeDamage(totleDamage);
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totleDamage)
    {
        int value = _targetStats.armor.GetValue();
        if(value >= totleDamage)
        {
            return 1;
        }
        else
        {
            return totleDamage - value; 
        }
    }

    protected virtual void TakeDamage(int _damage)
    {
        if (currentHealth < _damage)
            currentHealth = 0;
        else
            currentHealth -= _damage;
        
        if (currentHealth <= 0)
            Die();
    }
    protected virtual void Die()
    {

    }
}
