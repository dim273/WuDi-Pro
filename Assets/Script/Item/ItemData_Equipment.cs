using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Ring,
    Amulet
}
public enum SkillType
{
    Empty,
    Attack,
    Gain
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public SkillType skillType;         //武器技能类型
    public ItemEffect[] itemEffects;     //效果列表
    public float coolDown;          //该武器特殊效果的冷却时间

    [Header("Major stats")]
    public int strength;      //力量，每一点可以提供1点物理伤害
    public int agility;      //敏捷，每一点可以提供1%攻速加成(40%为上限)和1%暴击率加成(20%为上限)
    public int intellgence;   //智慧，每一点可以提供1魔法伤害和抗性
    public int vitality;     //体力，每一点可以提供生命加成

    [Header("Defensive stats")]
    public int maxHealth;          //最大生命
    public int armor;              //护甲值，每一点提供1伤害减免
    public int magicResisitance;   //魔法抗性，没一点提供2减免

    [Header("Damage stats")]
    public int damage;
    public int critChance;     //暴击几率
    public int critPower;      //暴击伤害

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftinggMaterials;
    public int descriptionLength;

    public void AddModifiers()
    {
        PlayerStat playerStats = PlayerManager.instance.player.GetComponent<PlayerStat>();
        playerStats.agility.AddModifier(agility);
        playerStats.strength.AddModifier(strength);
        playerStats.intellgence.AddModifier(intellgence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.magicResisitance.AddModifier(magicResisitance);
        playerStats.armor.AddModifier(armor);

        playerStats.damage.AddModifier(damage);
        playerStats.critPower.AddModifier(critPower);
        playerStats.critChance.AddModifier(critChance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStat playerstats = PlayerManager.instance.player.GetComponent<PlayerStat>();
        playerstats.agility.RemoveModifier(agility);
        playerstats.strength.RemoveModifier(strength);  
        playerstats.intellgence.RemoveModifier(intellgence);
        playerstats.vitality.RemoveModifier(vitality);

        playerstats.maxHealth.RemoveModifier(maxHealth);
        playerstats.armor.RemoveModifier(armor);
        playerstats.magicResisitance.RemoveModifier(magicResisitance);

        playerstats.damage.RemoveModifier(damage);  
        playerstats.critChance.RemoveModifier(critChance);
        playerstats.critPower.RemoveModifier(critPower);
        
        playerstats.fireDamage.RemoveModifier(fireDamage);
        playerstats.iceDamage.RemoveModifier(iceDamage);
        playerstats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public void ExcuteItemEffect(Transform _enemyPosition)
    {
        //执行效果
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "力量");
        AddItemDescription(agility, "灵巧");
        AddItemDescription(intellgence, "智慧");
        AddItemDescription(vitality, "体力");

        AddItemDescription(damage, "物理攻击");
        AddItemDescription(critPower, "暴击率");
        AddItemDescription(critChance, "暴击伤害");

        AddItemDescription(maxHealth, "生命值");
        AddItemDescription(armor, "护甲");
        AddItemDescription(magicResisitance, "魔抗");

        AddItemDescription(fireDamage, "火伤");
        AddItemDescription(iceDamage, "冰伤");
        AddItemDescription(lightingDamage, "雷伤");

        //使窗口拥有最小尺寸
        if(descriptionLength < 5)
        {
            for(int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        
        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if(_value != 0)
        {
            //Debug.Log(1);
            if(sb.Length >= 0) 
                sb.AppendLine();    //控制行数  
            if (_value > 0)
                sb.Append(" + " + _value + " " + _name);

            descriptionLength ++; 
        }
    }
}
