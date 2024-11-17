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
    private float defaultMoveSpeed;

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
        defaultMoveSpeed = moveSpeed;
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
        //直接冻住敌人
        if (_timeFreeze)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    public virtual void FreezeStart(float _duration) => StartCoroutine(FreezeTimeFor(_duration));
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

    public override void SlowEntityBy(float _slowPercentage, float _flowDuration)
    {
        defaultMoveSpeed = defaultMoveSpeed * (1 - _slowPercentage);
        anim.speed  = anim.speed * (1 - _slowPercentage);
        Invoke("ReturnDefaultSpeed", _flowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }
}
