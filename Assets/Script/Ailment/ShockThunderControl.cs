using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockThunderController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    private int damage;

    private Animator anim;
    private bool triggered;

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (!targetStats) return;
        if (triggered) return;
        triggered = true;
        Invoke("DamageAmdSelfDestroy", .3f);
        anim.SetTrigger("Hit");
    }
    private void DamageAmdSelfDestroy()
    {
        Destroy(gameObject, .2f);
        targetStats.TakeDamage(damage);
    }
}
