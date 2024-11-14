using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFireController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            //生成并造成伤害
            PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
            EnemyStat enemyStat = other.GetComponent<EnemyStat>();
            playerStat.DoMagicaDamage(enemyStat, 5);
        }
    }
}
