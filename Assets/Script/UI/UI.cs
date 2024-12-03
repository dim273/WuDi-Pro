using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillUI;
    [SerializeField] private GameObject carftUI;
    [SerializeField] private GameObject optionUI;

    public UI_SkillTooltip skillTooltip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_CraftWindow craftWindow;

    void Awake()
    {
        SwitchTo(skillUI);
    }

    void Start()
    {
        SwitchTo(null);
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            SwitchWithKeyTo(characterUI);

        if(Input.GetKeyDown(KeyCode.Alpha3))
            SwitchWithKeyTo(skillUI);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchWithKeyTo(carftUI);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchWithKeyTo(optionUI);

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

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }
        SwitchTo(_menu);
    }
}
