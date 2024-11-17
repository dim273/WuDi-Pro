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
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
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

    protected override void DecreaseHealth(int _damage)
    {
        base.DecreaseHealth(_damage);

        ItemData_Equipment armor = Inventory.instance.GetEquipment(EquipmentType.Armor);
        if (armor != null)
            armor.ExcuteItemEffect(player.transform);
    }
}
