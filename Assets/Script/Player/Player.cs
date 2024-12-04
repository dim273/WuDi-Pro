using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Player : Entity
{
    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashCoolDown;
    public float dashingTime;
    public int dashSpeed;
    private float dashTime;
    public float dashDir { get; private set; }
    public bool canDash;


    [Header("Attack Detail")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;

    [Header("技能影响")]
    [SerializeField] private UI_SkillTreeSlot moveSpeedButton;
    [SerializeField] private UI_SkillTreeSlot attackSpeedButton;
    public bool unlockAttackSpeedAdd {  get; private set; }

    public bool inBusy { get; private set; }
    public SkillManager skill { get; private set; }
    public GameObject sword;
    #region state
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSildeState wallState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackHoleState blackHole { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallState = new PlayerWallSildeState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHole = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;

        defaultJumpForce = jumpForce;
        defaultMoveSpeed = moveSpeed;

        moveSpeedButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockMoveSpeed);
        attackSpeedButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(UnlockAttackSpeed);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckDashInput();
        CreateChicken();
        AmuletUse();
    }

    private void CreateChicken()
    {
        if (Input.GetKeyDown(KeyCode.V) && skill.chicken.chickenSkillUnlocked)
            skill.chicken.CanUseSkill();
    }

    private void CheckDashInput()
    {
        dashTime -= Time.deltaTime;
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.CanUseSkill() && canDash)
        {
            //dashTime = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;
            stateMachine.ChangeState(dashState);
        }
    }
    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void ClearTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationTriggerCalled();
    public IEnumerator BusyFor(float _seconds)
    {
        inBusy = true;
        yield return new WaitForSeconds(_seconds);
        inBusy = false;
    }
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deathState);
    }

    public override void SlowEntityBy(float _slowPercentage, float _flowDuration)
    {
        jumpForce = jumpForce * (1 - _slowPercentage);
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _flowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        jumpForce = defaultJumpForce;
        moveSpeed = defaultMoveSpeed;
    }
    private void AmuletUse()
    {
        if (!stats.isDead && Input.GetKeyDown(KeyCode.F))
        {
            //Debug.Log("1");
            ItemData_Equipment amulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
            if (amulet == null) return;
            amulet.ExcuteItemEffect(transform);
        }
    }

    private void UnlockMoveSpeed()
    {
        //改变其速度的技能效果
        if (moveSpeedButton.unlocked)
            moveSpeed = moveSpeed * 1.2f;
    }

    private void UnlockAttackSpeed()
    {
        //改变其攻击速度的技能效果
        if (attackSpeedButton.unlocked)
            unlockAttackSpeedAdd = true;
    }
}
