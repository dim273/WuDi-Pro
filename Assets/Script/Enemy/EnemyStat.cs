using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStats
{
    private Enemy enemy;

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
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
        enemy.Damage();
    }
}
