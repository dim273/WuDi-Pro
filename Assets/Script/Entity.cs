using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region componenment
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX entityFX { get; private set; }
    #endregion

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groudCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir = 1;
    protected bool isFacingRight = true;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        entityFX = GetComponentInChildren<EntityFX>();
        anim = GetComponentInChildren<Animator>();
    }
    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        entityFX.StartCoroutine("FlashFX");
        Debug.Log(gameObject.name + "was damaged.");
    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groudCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groudCheck.position, new Vector3(groudCheck.position.x, groudCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region FilpManage
    public void Filp()
    {
        facingDir = -1 * facingDir;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    public void FilpController(float _x)
    {
        if (_x > 0 && !isFacingRight)
            Filp();
        else if (_x < 0 && isFacingRight)
            Filp();
    }
    #endregion

    #region Velocity
    public void SetVelocity(float _x_velocity, float _y_velocity)
    {
        rb.velocity = new Vector2(_x_velocity, _y_velocity);
        FilpController(_x_velocity);
    }
    public void ZeroVelocity()
    {
        rb.velocity = new Vector2(0, 0);
    }
    #endregion
}
