using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontsize;

    public void ShowSkillToolTip(string _skillName, string _skillDescription, string _skillCost)
    {
        skillDescription.text = _skillDescription;
        skillName.text = _skillName;
        skillCost.text = "»¨·Ñ£º" + _skillCost;
        gameObject.SetActive(true);
    }

    public void HideSkillToolTip()
    {
        skillName.fontSize = defaultNameFontsize;
        gameObject.SetActive(false);
    }
}
