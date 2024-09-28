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
        if(canRotate)
            transform.right = rb.velocity;

        if (isReturn)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, player.transform.position) < 1.5f)
            {
                player.ClearTheSword();
            }
        }
    }
    public void ReturnSword()
    {
        rb.isKinematic = true;
        transform.parent = null;
        isReturn = true;
    }
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;
    }
}
