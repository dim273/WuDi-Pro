using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStats
{
    private Enemy enemy;

    public override void DoDamage(CharacterStats _targetStats, int baseDamage)
    {
        base.DoDamage(_targetStats, baseDamage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }

    protected override void Start()
    {
        enemy = GetComponent<Enemy>();
        base.Start();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
}
