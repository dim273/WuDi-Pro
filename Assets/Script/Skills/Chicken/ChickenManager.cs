using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float chickenExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 3;
    private Transform closestEnemy;

    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupChicken(float _chickenExistTimer, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy)
    {
        chickenExistTimer = _chickenExistTimer;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }
    private void Update()
    {
        chickenExistTimer -= Time.deltaTime;
        if (chickenExistTimer < 0)
            FinishChicken();

        if (canMove)
        {
            if (closestEnemy != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, closestEnemy.position) < 1)
                {
                    canMove = false;
                    FinishChicken();
                }
            }
        }

        if(canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(2, 2), growSpeed * Time.deltaTime);
    }
    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach(Collider2D hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }
    public void FinishChicken()
    {
        if(canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            DestroySelf();
    }
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (colliders.Length > 0)
        {
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }
    private void DestroySelf() => Destroy(gameObject);
}
