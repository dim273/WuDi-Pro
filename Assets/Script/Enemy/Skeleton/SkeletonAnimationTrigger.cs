using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SkeletonAnimationTrigger : MonoBehaviour
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach(var hit in collider2Ds)
        {
            if(hit.GetComponent<Player>() != null)
            {
                PlayerStat target = hit.GetComponent<PlayerStat>();
                enemy.stats.DoDamage(target);
            }
        }
    }
    private void SlowSpeed()
    {
        enemy.anim.speed = 0.5f;
        StartCoroutine("RecoverSpeed");
    }
    private IEnumerator RecoverSpeed()
    {
        yield return new WaitForSeconds(.5f);
        enemy.anim.speed = 1f;
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() =>enemy.CloseCounterAttackWindow();

    private void DestroyThis()
    {
        enemy.DestroyThis();
    }
}
