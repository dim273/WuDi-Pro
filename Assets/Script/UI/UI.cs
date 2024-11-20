using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    public UI_ItemTooltip itemTooltip;

    public void Start()
    {
        itemTooltip = characterUI.GetComponentInChildren<UI_ItemTooltip>();
    }

    public void SwitchTo(GameObject _menu)
    {
        //菜单间进行切换
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
        }
    }
}
