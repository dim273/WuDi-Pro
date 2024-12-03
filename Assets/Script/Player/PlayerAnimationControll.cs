using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationControll : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();
    private Animator anim => GetComponent<Animator>();

    private void Update()
    {
       
    }
    private void AnimationTriggers()
    {
        player.AnimationTrigger();
        anim.speed = 1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach(var hit in collider2Ds)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                EnemyStat enemyStat = hit.GetComponent<EnemyStat>();
                if (enemyStat == null) return;
                player.stats.DoDamage(enemyStat, 0);

                //��ȴ���˲���װ���������м������ͷż���
                Inventory.instance.UseEquipmentSkillOnAttack(enemyStat.transform);
            }
        }
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
