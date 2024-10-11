using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;      //������ÿһ������ṩ1�������˺�
    public Stat agility;      //���ݣ�ÿһ������ṩ1%���ټӳ�(40%Ϊ����)��1%�����ʼӳ�(20%Ϊ����)
    public Stat intelgence;   //�ǻۣ�ÿһ������ṩ1ħ���˺�
    public Stat vitality;     //������ÿһ������ṩ�����ӳ�

    [Header("Defensive stats")]
    public Stat maxHealth;      //�������
    public Stat armor;          //����ֵ��ÿһ���ṩ1�˺�����

    [Header("Damage stats")]
    public Stat damage;
    public Stat critChance;     //��������
    public Stat critPower;      //�����˺�

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
        _targetStats.TakeDamage(totleDamage);
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
