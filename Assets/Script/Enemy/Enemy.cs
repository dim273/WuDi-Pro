using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Stunned Info")]
    public float stunnedDuration;
    public Vector2 stunnedDirection;
    [SerializeField] protected GameObject counterWindow;
    protected bool canBeStunned;

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float initMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    public string lastingAnimBoolName {  get; protected set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastingAnimBoolName = _animBoolName;
    }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        initMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterWindow.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterWindow.SetActive(false);
    }
    public virtual bool IsStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    public virtual void FreezeTime(bool _timeFreeze)
    {
        if (_timeFreeze)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = initMoveSpeed;
            anim.speed = 1;
        }
    }
    protected virtual IEnumerator FreezeTimeFor(float _time)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_time);
        FreezeTime(false);
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + attackDistance * facingDir, wallCheck.position.y));
    }
}
