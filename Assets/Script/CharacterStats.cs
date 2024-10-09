using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat strenth;
    public Stat damage;
    public Stat maxHealth;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
    }
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totleDamage = damage.GetValue() + strenth.GetValue();
        _targetStats.TakeDamage(totleDamage);
    }
    protected virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(_damage);
        if (currentHealth < 0)
            Die();
    }
    protected virtual void Die()
    {

    }
}
