using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkeleton enemy;
    private Transform player;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        stateTimer = enemy.battleTime;
        //Debug.Log("I am in this;");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if(enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack())
                enemy.stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if(stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                enemyStateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;
        //敌人在到Player一定距离后会停下来
        if (enemy.IsGroundDetected() && Mathf.Abs(enemy.transform.position.x - player.transform.position.x) > 0.5)
            enemy.SetVelocity(moveDir * enemy.moveSpeed, enemy.rb.velocity.y);
        else
        {
            if(moveDir != enemy.facingDir)
                enemy.SetVelocity(moveDir * enemy.moveSpeed, enemy.rb.velocity.y);
            else
                enemy.ZeroVelocity();
        }      
    }
    private bool CanAttack()
    {
        if (Time.time > enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
