using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool unlocked;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;

    private Image skillImage;
    private UI ui;

    private void OnValidate()
    {
        gameObject.name = "Skill - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        ui = GetComponentInParent<UI>();
        skillImage = GetComponent<Image>();
        skillImage.color = lockedColor;
    }

    public void UnlockSkillSlot()
    {
        for(int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if(shouldBeUnlocked[i].unlocked == false)
            {
                //Debug.Log("Cannot unlock skill");
                return;
            }
        }
        for(int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                //Debug.Log("Cannot unlock skill");
                return; 
            }
        }

        if (!PlayerManager.instance.HaveEnoughSoul(skillCost))
            return;

        unlocked = true;
        skillImage.color = Color.white;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowSkillToolTip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideSkillToolTip();
    }

}
