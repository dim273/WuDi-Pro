using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength;      //力量，每一点可以提供1点物理伤害
    public Stat agility;      //敏捷，每一点可以提供1%攻速加成(40%为上限)和1%暴击率加成(20%为上限)
    public Stat intellgence;   //智慧，每一点可以提供1魔法伤害和抗性
    public Stat vitality;     //体力，每一点可以提供生命加成

    [Header("Defensive stats")]
    public Stat maxHealth;          //最大生命
    public Stat armor;              //护甲值，每一点提供1伤害减免
    public Stat magicResisitance;   //魔法抗性，没一点提供2减免

    [Header("Damage stats")]
    public Stat damage;
    public Stat critChance;     //暴击几率
    public Stat critPower;      //暴击伤害

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;
    public bool isChilded;
    public bool isShocked;

    [SerializeField] private float ailmentDuration = 3;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ignitedCoolDown = .3f;
    private float ignitDamageTimer;
    private int ignitDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public System.Action onHealthChanged;
    public bool isDead = false;

    [SerializeField] public int currentHealth;

    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        critPower.SetDefaultValue(150);
        fx = GetComponentInChildren<EntityFX>();
    }
    protected virtual void Update()
    {
        ignitDamageTimer -= Time.deltaTime;

        shockedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        ignitedTimer -= Time.deltaTime;

        if (shockedTimer < 0)
            isShocked = false;

        if (chilledTimer < 0)
            isChilded = false;

        if (ignitedTimer < 0)
            isIgnited = false;

        if(isIgnited && !isDead)
            ApplyIgnited();

    }

    private void ApplyIgnited()
    {
        if (ignitDamageTimer < 0)
        {
            DecreaseHealth(ignitDamage);
            if (currentHealth <= 0)
            {
                Die();
            }
            ignitDamageTimer = ignitedCoolDown;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats, int baseDamage)
    {
        int totleDamage = damage.GetValue() + strength.GetValue() + baseDamage;
        if (CanCrit())
        {
            totleDamage = CalculateCriticalDamage(totleDamage);
        }
        totleDamage = CheckTargetArmor(_targetStats, totleDamage);
        _targetStats.TakeDamage(totleDamage);
        //DoMagicaDamage(_targetStats);
    }

    public virtual void DoMagicaDamage(CharacterStats _targetStats, int addDamage)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();
        int totleMagicaDamage = _fireDamage + _iceDamage + _lightingDamage + addDamage;

        totleMagicaDamage = CheckTargetResistance(_targetStats,totleMagicaDamage);
        _targetStats.TakeDamage(totleMagicaDamage);

        if(Mathf.Max(_lightingDamage, _fireDamage, _iceDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        //如果出现两种伤害相同时的处理方法
        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            int ch = Random.Range(0, 3);
            if(ch == 0 && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            else if(ch == 1 && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            else if(ch == 2 && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }
        if (canApplyIgnite)
        {
            int _damage = Mathf.RoundToInt(_fireDamage * .15f);
            _damage = Mathf.Max(1, _damage);
            _targetStats.SetIgniteDamage(Mathf.RoundToInt(_damage));
        }
        else if (canApplyShock)
            _targetStats.SetShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .15f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private int CheckTargetResistance(CharacterStats targetStats, int totleMagicaDamage)
    {
        //计算魔法伤害
        totleMagicaDamage -= (targetStats.magicResisitance.GetValue() * 2 + targetStats.intellgence.GetValue());
        totleMagicaDamage = Mathf.Clamp(totleMagicaDamage, 0, int.MaxValue);
        return totleMagicaDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilded && !isShocked;
        bool canApplyChill = !isIgnited && !isChilded && !isShocked;
        bool canApplyShock = !isIgnited && !isChilded;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentDuration;
            fx.IgniteFxFor(ailmentDuration);
        }
        if (_chill && canApplyChill)
        {
            isChilded = _chill;
            chilledTimer = ailmentDuration;
            float slowPercent = .2f;
            transform.GetComponent<Entity>().SlowEntityBy(slowPercent, ailmentDuration);
            fx.ChillFxFor(ailmentDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                isShocked = true;
                
                shockedTimer = ailmentDuration;
                fx.ShockFxFor(ailmentDuration);
            }
            else
            {
                if (GetComponent<Player>() != null) return;
                GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
                newShockStrike.GetComponent<ShockThunderController>().Setup(shockDamage, this);
            }
        }
    }

    private int CalculateCriticalDamage(int _damage)
    {
        //检测暴击伤害
        float totleCirticalPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totleCirticalPower;

        return Mathf.RoundToInt(critDamage);
    }

    private bool CanCrit()
    {
        int totChanceToCrit = critChance.GetValue() + agility.GetValue();
        if(Random.Range(0, 100) > totChanceToCrit) return false;
        return true;
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totleDamage)
    {
        int value = _targetStats.armor.GetValue();
        if (_targetStats.isShocked)
            value = (int)((int)value * 0.8);

        if(value >= totleDamage)
            return 1;
        else
            return totleDamage - value; 
    }

    public virtual void TakeDamage(int _damage)
    {
        //Debug.Log(_damage);
        fx.StartCoroutine("FlashFX");
        DecreaseHealth(_damage);

        if (currentHealth <= 0 && !isDead)
            Die();
    }

    protected virtual void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;
        if(onHealthChanged != null)
            onHealthChanged();
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public void SetIgniteDamage(int _damage) => ignitDamage = _damage;

    public void SetShockStrikeDamage(int _damage) => shockDamage = _damage;

    protected virtual void Die()
    {
        isDead = true;
    }

}
