using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWallDetected())
        {
            if (xInput != 0 && xInput != player.facingDir)
                stateMachine.ChangeState(player.moveState);
        }
        else
        {
            if (xInput != 0)
                stateMachine.ChangeState(player.moveState);
        }
    }
}
