using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and fire", menuName = "Data/Item effect/Ice and fire")]
public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject IceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        Player player = PlayerManager.instance.player;

        bool thridAttack = player.GetComponent<Player>().primaryAttack.comboCounter == 2;

        if (thridAttack)
        {
            GameObject newIceAndFire = Instantiate(IceAndFirePrefab, _enemyPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2 (xVelocity * player.facingDir, 0);
            Destroy(newIceAndFire, 1f);
        }

    }

}
