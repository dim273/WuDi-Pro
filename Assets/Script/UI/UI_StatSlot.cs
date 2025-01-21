using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private StatType statType;
    [SerializeField] private string statName = "empty";
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat-" + statType.ToString();

        if(statValueText != null && statName == "empty")
        {
            //statNameText.text = statType.ToString();
            switch (statType)
            {
                case StatType.strength: statNameText.text = "����"; break;
                case StatType.agility: statNameText.text = "����"; break;
                case StatType.intelligence: statNameText.text = "�ǻ�"; break;
                case StatType.vitality: statNameText.text = "����"; break;

                case StatType.damage: statNameText.text = "������"; break;
                case StatType.critChance: statNameText.text = "������"; break;
                case StatType.critPower: statNameText.text = "�����˺�"; break;
                case StatType.armor: statNameText.text = "����"; break;
                case StatType.magicRes: statNameText.text = "ħ��"; break;

                case StatType.fire: statNameText.text = "����"; break;
                case StatType.ice: statNameText.text = "����"; break;
                case StatType.lighting: statNameText.text = "����"; break;
                case StatType.health: statNameText.text = "����"; break;
            }
        }
        else if(statValueText != null)
        {
            statValueText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStat playerStats = PlayerManager.instance.player.GetComponent<PlayerStat>();
        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStats(statType).GetValue().ToString();

            if (statType == StatType.health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if (statType == StatType.damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.critChance) 
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.magicRes)
                statValueText.text = (playerStats.magicResisitance.GetValue() + playerStats.intellgence.GetValue()).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatToolTip();
    }

}
