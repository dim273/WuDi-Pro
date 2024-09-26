using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected EnemySkeleton enemy;
    protected Transform player;
    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            enemy.stateMachine.ChangeState(enemy.battleState);
    }
}
