using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;
    
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if(cloneTimer < 0)
            sr.color = new Color(1,1,1,sr.color.a - (Time.deltaTime * colorLosingSpeed));

        if(sr.color.a < 0)
            Destroy(gameObject);
    }
    public void SetupClone(Transform _cloneTransform, float _cloneDuration, bool _canAttack)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackNum", Random.Range(1, 4));
        }
        cloneTimer = _cloneDuration; 
        transform.position = _cloneTransform.position;
        FaseCloseEnemy();
    }
    private void AnimationTriggers()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }
    private void FaseCloseEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        foreach(var hit in  colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        if(closestEnemy != null)
        {
            if(transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
