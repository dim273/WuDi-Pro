using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    private Player player;

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
    }

    protected override void Die()
    {
        base.Die();
    }

    protected override void Start()
    {
        player = GetComponent<Player>();
        base.Start();
    }

    protected override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        player.Damage();
    }
}
