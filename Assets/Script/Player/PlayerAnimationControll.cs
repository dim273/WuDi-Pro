using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationControll : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private float equipmentCD;  //装备技能的冷却


    private void Update()
    {
        equipmentCD -= Time.deltaTime;
    }
    private void AnimationTriggers()
    {
        player.AnimationTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach(var hit in collider2Ds)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStat enemyStat = hit.GetComponent<EnemyStat>();
                player.stats.DoDamage(enemyStat, 0);
                if (Inventory.instance.GetEquipment(EquipmentType.Weapon) != null && equipmentCD < 0)
                {
                    Inventory.instance.GetEquipment(EquipmentType.Weapon).ExcuteItemEffect(enemyStat.transform);
                    equipmentCD = Inventory.instance.GetEquipment(EquipmentType.Weapon).coolDown;
                }
            }
        }
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
