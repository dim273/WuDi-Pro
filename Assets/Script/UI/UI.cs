using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class UI : MonoBehaviour
{
    [Header("结束黑屏")]
    [SerializeField] private UI_Fade fadeScreen;
    [SerializeField] private GameObject endText;
    [Space]


    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillUI;
    [SerializeField] private GameObject carftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;

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
        SwitchTo(inGameUI);
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
            //检查UI界面是否含有fadeScreen
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_Fade>() != null;
            if(!fadeScreen)
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
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }
    private void CheckForInGameUI()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }
        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        Debug.Log("死亡");
        SwitchTo(null);
        fadeScreen.FadeOut();
        StartCoroutine(EndTextShow());
    }

    IEnumerator EndTextShow()
    {
        yield return new WaitForSeconds(1.5f);
        endText.SetActive(true);
    }
}
