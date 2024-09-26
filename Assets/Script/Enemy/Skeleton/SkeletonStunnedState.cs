using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemy;
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.entityFX.InvokeRepeating("RedColorBlink", 0, .1f);
        stateTimer = enemy.stunnedDuration;
        enemy.rb.velocity = new Vector2(-enemy.facingDir * enemy.stunnedDirection.x, enemy.stunnedDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.entityFX.Invoke("CancelRedBlink", 0);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer < 0)
            enemyStateMachine.ChangeState(enemy.idleState);
    }
}
