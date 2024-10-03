using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.clone.CreateClone(player.transform, new Vector3(0,0,0));
        stateTimer = player.dashingTime;
    }

    public override void Exit()
    {
        player.SetVelocity(0, rb.velocity.y);
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.IsWallDetected() && !player.IsGroundDetected())
            stateMachine.ChangeState(player.wallState);

        player.SetVelocity(player.dashDir * player.dashSpeed, 0);

        if (stateTimer < -0.1f)
            stateMachine.ChangeState(player.idleState);
    }
}
