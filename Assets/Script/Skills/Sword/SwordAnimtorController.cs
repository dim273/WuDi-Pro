using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimtorController : MonoBehaviour
{
    private SwordManager swordManager => GetComponentInParent<SwordManager>();
    private Player player;
    private void Start()
    {
        player = swordManager.player;
    }
    private void ReturnSword()
    {
        swordManager.ReturnSword();
    } 
    private void ExplodeAttack()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, swordManager.explodeRange);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicaDamage(hit.GetComponent<EnemyStat>(), 0);
            }
        }
    }
}
