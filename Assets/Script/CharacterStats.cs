using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;      //力量，每一点可以提供1点物理伤害
    public Stat agility;      //敏捷，每一点可以提供1%攻速加成(40%为上限)和1%暴击率加成(20%为上限)
    public Stat intellgence;   //智慧，每一点可以提供1魔法伤害和抗性
    public Stat vitality;     //体力，每一点可以提供生命加成

    [Header("Defensive stats")]
    public Stat maxHealth;      //最大生命
    public Stat armor;          //护甲值，每一点提供1伤害减免
    public Stat magicResisitance;   //魔法抗性，没一点提供2减免

    [Header("Damage stats")]
    public Stat damage;
    public Stat critChance;     //暴击几率
    public Stat critPower;      //暴击伤害

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;
    public bool isChilded;
    public bool isShocked;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
        critPower.SetDefaultValue(150);
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totleDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totleDamage = CalculateCriticalDamage(totleDamage);
        }
        totleDamage = CheckTargetArmor(_targetStats, totleDamage);
        //_targetStats.TakeDamage(totleDamage);
        DoMagicaDamage(_targetStats);
    }

    public virtual void DoMagicaDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();
        int totleMagicaDamage = _fireDamage + _iceDamage + _lightingDamage;

        totleMagicaDamage = CheckTargetResistance(_targetStats,totleMagicaDamage);
        _targetStats.TakeDamage(totleMagicaDamage);
    }

    private static int CheckTargetResistance(CharacterStats targetStats, int totleMagicaDamage)
    {
        totleMagicaDamage -= (targetStats.magicResisitance.GetValue() * 2 + targetStats.intellgence.GetValue());
        totleMagicaDamage = Mathf.Clamp(totleMagicaDamage, 0, int.MaxValue);
        return totleMagicaDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if(isChilded || isShocked || isIgnited)
        {
            return;
        }

        isIgnited = _ignite;
        isChilded = _chill;
        isShocked = _shock;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totleCirticalPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totleCirticalPower;

        return Mathf.RoundToInt(critDamage);
    }

    private bool CanCrit()
    {
        int totChanceToCrit = critChance.GetValue() + agility.GetValue();
        if(Random.Range(0, 100) > totChanceToCrit) return false;
        return true;
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
        //Debug.Log(_damage);
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
