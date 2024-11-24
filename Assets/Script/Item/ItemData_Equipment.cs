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
    public SkillType skillType;         //������������
    public ItemEffect[] itemEffects;     //Ч���б�
    public float coolDown;          //����������Ч������ȴʱ��

    [Header("Major stats")]
    public int strength;      //������ÿһ������ṩ1�������˺�
    public int agility;      //���ݣ�ÿһ������ṩ1%���ټӳ�(40%Ϊ����)��1%�����ʼӳ�(20%Ϊ����)
    public int intellgence;   //�ǻۣ�ÿһ������ṩ1ħ���˺��Ϳ���
    public int vitality;     //������ÿһ������ṩ�����ӳ�

    [Header("Defensive stats")]
    public int maxHealth;          //�������
    public int armor;              //����ֵ��ÿһ���ṩ1�˺�����
    public int magicResisitance;   //ħ�����ԣ�ûһ���ṩ2����

    [Header("Damage stats")]
    public int damage;
    public int critChance;     //��������
    public int critPower;      //�����˺�

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
        //ִ��Ч��
        foreach(var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "����");
        AddItemDescription(agility, "����");
        AddItemDescription(intellgence, "�ǻ�");
        AddItemDescription(vitality, "����");

        AddItemDescription(damage, "������");
        AddItemDescription(critPower, "������");
        AddItemDescription(critChance, "�����˺�");

        AddItemDescription(maxHealth, "����ֵ");
        AddItemDescription(armor, "����");
        AddItemDescription(magicResisitance, "ħ��");

        AddItemDescription(fireDamage, "����");
        AddItemDescription(iceDamage, "����");
        AddItemDescription(lightingDamage, "����");

        //ʹ����ӵ����С�ߴ�
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
                sb.AppendLine();    //��������  
            if (_value > 0)
                sb.Append(" + " + _value + " " + _name);

            descriptionLength ++; 
        }
    }
}
