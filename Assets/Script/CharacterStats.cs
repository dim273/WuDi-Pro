using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;      //������ÿһ������ṩ1�������˺�
    public Stat agility;      //���ݣ�ÿһ������ṩ1%���ټӳ�(40%Ϊ����)��1%�����ʼӳ�(20%Ϊ����)
    public Stat intellgence;   //�ǻۣ�ÿһ������ṩ1ħ���˺��Ϳ���
    public Stat vitality;     //������ÿһ������ṩ�����ӳ�

    [Header("Defensive stats")]
    public Stat maxHealth;      //�������
    public Stat armor;          //����ֵ��ÿһ���ṩ1�˺�����
    public Stat magicResisitance;   //ħ�����ԣ�ûһ���ṩ2����

    [Header("Damage stats")]
    public Stat damage;
    public Stat critChance;     //��������
    public Stat critPower;      //�����˺�

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
