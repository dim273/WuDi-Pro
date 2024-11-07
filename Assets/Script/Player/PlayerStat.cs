using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : CharacterStats
{
    private Player player;

    public override void DoDamage(CharacterStats _targetStats, int baseDamage)
    {
        base.DoDamage(_targetStats, baseDamage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
    }

    protected override void Start()
    {
        player = GetComponent<Player>();
        base.Start();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }
}
