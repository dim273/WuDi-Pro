using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    protected Animator anim;

    protected float xInput;
    protected float yInput;
    protected float stateTimer;
    protected bool triggerCalled;

    private string animBoolName;


    public PlayerState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName)
    {
        this.stateMachine = _playerStateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        triggerCalled = false;
        rb = player.rb;
    }
    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        stateTimer -= Time.deltaTime;
        player.anim.SetFloat("yVelocity", player.rb.velocity.y);
    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
        //Debug.Log("I exit the " + animBoolName);
    }
    public virtual void AnimationTriggerCalled()
    {
        triggerCalled = true;
    }
}
