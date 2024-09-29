using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;

        if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Filp();
        else if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Filp();

        //����ʽ�ӿ��������������ĳ�������˴�����Ҫ����ע��
        //rb.velocity = new Vector2(10 * -player.facingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
