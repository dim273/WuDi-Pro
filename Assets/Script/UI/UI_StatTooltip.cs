using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStatToolTip(string _text)
    {
        if (_text == null) return;
        description.text = _text;
        AdjustPosition();
        gameObject.SetActive(true);
        
    }

    public void HideStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
