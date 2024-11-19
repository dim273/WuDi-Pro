using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private StatType statType;
    [SerializeField] private string statName = "empty";
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statType.ToString();

        if(statValueText != null && statName == "empty")
        {
            statNameText.text = statType.ToString();
        }
        else if(statValueText != null)
        {
            statValueText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
    }

    public void UpdateStatValueUI()
    {
        PlayerStat playerStats = PlayerManager.instance.player.GetComponent<PlayerStat>();
        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStats(statType).GetValue().ToString();
        }
    }
}
