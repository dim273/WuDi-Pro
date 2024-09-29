using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    [SerializeField] private float returnSpeed;
    private bool isReturn;

    //private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;

    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBouncing;
    private List<Transform> enemyTargets;
    private int enemyIndex;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    private void Start()
    {
        //anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        //if(canRotate)
        //    transform.right = rb.velocity;

        if (isReturn)
        {
            Vector2 position = new Vector2(player.transform.position.x, player.transform.position.y + 0.8f);
            transform.position = Vector2.MoveTowards(transform.position, position, returnSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, position) < 0.6f)
            {
                player.ClearTheSword();
            }
        }
        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[enemyIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTargets[enemyIndex].position) < .1f)
            {
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

        //篮球不会黏在物体上，所以注释掉，但可用于箭之类的东西
        //transform.parent = null;

        isReturn = true;
    }
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }
    public void SetupBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        amountOfBouncing = _amountOfBounce;
        enemyTargets = new List<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn)
            return;
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0) 
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach(Collider2D hit in colliders)
                {
                    if(hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(hit.transform);
                    }
                }
            }
        }
        StuckInto(collision);
    }
    private void StuckInto(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;
        //rb.isKinematic = true;
        if (isBouncing && enemyTargets.Count > 0)
            return;
        rb.velocity = new Vector2(0f, 0f);
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //transform.parent = collision.transform;
    }
}
