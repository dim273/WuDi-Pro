using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    //敌人等级设置
    [Header("Level details")]
    [SerializeField] private int level = 1;
    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = .4f;
    public override void DoDamage(CharacterStats _targetStats, int baseDamage)
    {
        base.DoDamage(_targetStats, baseDamage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        myDropSystem.GenerateDrop();
    }

    protected override void Start()
    {
        ApplyLevelModifier();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
        base.Start();
    }

    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intellgence);
        Modify(vitality);

        Modify(damage);
        Modify(maxHealth);
        Modify(armor);
        Modify(magicResisitance);
    }

    private void Modify(Stat _stat)
    {
        for(int i = 1; i <= level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
}
