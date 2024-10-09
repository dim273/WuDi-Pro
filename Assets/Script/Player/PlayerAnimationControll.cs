using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimationControll : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

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
                player.stats.DoDamage(enemyStat);
            }
        }
    }
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
