using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter;
    private float lastTimeAttack;
    private float comboWindow = 1.2f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = .2f;
        xInput = 0;
        if(comboCounter > 2 || Time.time >= lastTimeAttack + comboWindow) 
            comboCounter = 0;

        float attackDir = player.facingDir;
        if(xInput != 0)
            attackDir = xInput;
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        player.anim.SetInteger("comboCounter", comboCounter);
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        if(stateTimer < 0)
            player.ZeroVelocity();
        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
        base.Update();
    }
}
