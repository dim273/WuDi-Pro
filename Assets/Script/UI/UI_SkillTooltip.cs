using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;

    public void ShowSkillToolTip(string _skillName, string _skillDescription)
    {
        skillDescription.text = _skillDescription;
        skillName.text = _skillName;
        gameObject.SetActive(true);
    }

    public void HideSkillToolTip()
    {
        gameObject.SetActive(false);
    }
}
