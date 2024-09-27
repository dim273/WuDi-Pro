using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSildeState : PlayerState
{
    public PlayerWallSildeState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if(!player.IsWallDetected())
            stateMachine.ChangeState(player.airState);

        if(yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y * .7f);

        if (xInput != 0 && player.facingDir != xInput )
            stateMachine.ChangeState(player.idleState);
    }
}
