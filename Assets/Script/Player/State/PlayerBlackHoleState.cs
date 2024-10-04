using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void AnimationTriggerCalled()
    {
        base.AnimationTriggerCalled();
    }

    public override void Enter()
    {
        base.Enter();
        skillUsed = false;
        stateTimer = flyTime;
        defaultGravity = player.rb.gravityScale;
        player.rb.gravityScale = 0;
        player.canDash = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        player.canDash = true;
        player.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer > 0)
        {
            player.rb.velocity = new Vector2(0, 6);
        }
        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);
            if(!skillUsed)
            {
                if(player.skill.blackHole.CanUseSkill())
                    skillUsed = true;
            }
        }
        if(player.skill.blackHole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }
}
