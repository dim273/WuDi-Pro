using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    private float returnSpeed;
    private bool isReturn;
    private float ensureTime = 2f;

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;

    [Header("Bounce Info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBouncing;
    private List<Transform> enemyTargets;
    private int enemyIndex;

    [Header("Pierce Info")]
    private int pierceAmount;

    [Header("Explode Info")]
    public float explodeRange;
    private bool isExplode = false;

    private float freezeTimeDuration;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("Explode", false);
        Destroy(gameObject, 7f);
    }
    private void Update()
    {
        //if(canRotate)
        //    transform.right = rb.velocity;
        ensureTime -= Time.deltaTime;
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.y + 0.8f);
        if (Vector2.Distance(transform.position, position) < 1.5f && ensureTime < 0)
            Destroy(gameObject);

        if (isReturn)
        {
            transform.position = Vector2.MoveTowards(transform.position, position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, position) < 0.6f)
            {
                player.ClearTheSword();
            }
        }
        else
        {
            BounceLogic();
        }
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[enemyIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTargets[enemyIndex].position) < .1f)
            {
                enemyTargets[enemyIndex].GetComponent<Enemy>().Damage();
                enemyIndex++;
                amountOfBouncing--;
                if (amountOfBouncing == 0)
                {
                    isBouncing = false;
                    isReturn = true;
                }
                if (enemyIndex >= enemyTargets.Count)
                {
                    enemyIndex = 0;
                }
            }
        }
    }

    public void ReturnSword()
    {
        //返回时的运动与刚体无关，故可以冻结刚体
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("Explode", false);
        //篮球不会黏在物体上，所以注释掉，但可用于箭之类的东西
        //transform.parent = null;

        isReturn = true;
    }
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTime, float _returnSpeed)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        freezeTimeDuration = _freezeTime;
        returnSpeed = _returnSpeed;
    }
    public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBouncing = _amountOfBounce;
        bounceSpeed = _bounceSpeed;
        enemyTargets = new List<Transform>();
    }
    public void SetupExplode(bool _isExplode, float _explodeRange)
    {
        explodeRange = _explodeRange;
        isExplode = _isExplode;
    }
    public void SetUpFierce(int _amountOfFierce)
    {
        pierceAmount = _amountOfFierce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn)
            return;
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Damage();
            if (!isBouncing && pierceAmount <= 0 && !isExplode)
            {
                enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
            } 
            else if(!isBouncing && pierceAmount <= 0 && isExplode)
            {
                anim.SetBool("Explode", true);
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        BounceAttack(collision);
        StuckInto(collision);
    }
    private void BounceAttack(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (Collider2D hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        
        //rb.isKinematic = true;
        canRotate = false;
        cd.enabled = false;
        if (isBouncing && enemyTargets.Count > 0)
            return;
        rb.velocity = new Vector2(0f, 0f);
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //transform.parent = collision.transform;
    }
}
