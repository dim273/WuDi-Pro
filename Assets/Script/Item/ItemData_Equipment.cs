using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
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
}
