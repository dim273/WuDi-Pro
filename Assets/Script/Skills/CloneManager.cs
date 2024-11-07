using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius;
    private Transform closestEnemy;

    private bool canDuplicateClone;
    private float chanceToDuplicate;
    private int facingDir = 1;

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
    public void SetupClone(Transform _cloneTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _closestEnemy,
                            bool _canDuplicateClone, float _chanceToDuplicate, Player _player)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackNum", Random.Range(1, 4));
        }
        cloneTimer = _cloneDuration; 
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
        player = _player;
        transform.position = _cloneTransform.position + _offset;
        FacingCloseEnemy();
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
                player.stats.DoDamage(hit.GetComponent<EnemyStat>(), 0);
                if (canDuplicateClone)
                {
                    if(Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }
    private void FacingCloseEnemy()
    {
        if(closestEnemy != null)
        {
            if(transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
