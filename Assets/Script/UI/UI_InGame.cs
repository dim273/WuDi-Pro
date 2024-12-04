using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image chickenImage;
    [SerializeField] private Image basketballImage;
    [SerializeField] private Image amuletImage;

    [SerializeField] private TextMeshProUGUI currentSoul;
    private SkillManager skill;

    void Start()
    {
        if (playerStat != null) 
            playerStat.onHealthChanged += UpdateHealthUI;

        skill = SkillManager.instance;
    }

    void Update()
    {
        currentSoul.text = PlayerManager.instance.soul.ToString("#,#");

        if(Input.GetKeyDown(KeyCode.LeftShift))
            SetCoolDown(dashImage);
        if(Input.GetKeyDown(KeyCode.M) && skill.blackHole.unlockBlackHole)
            SetCoolDown(basketballImage);
        if(Input.GetKeyDown(KeyCode.V) && skill.chicken.chickenSkillUnlocked)
            SetCoolDown(chickenImage);
        if(Input.GetKeyDown(KeyCode.F) && Inventory.instance.GetEquipment(EquipmentType.Amulet))
            SetCoolDown(amuletImage);


        CheckCoolDown(dashImage, skill.dash.coolDown);
        CheckCoolDown(basketballImage, skill.blackHole.coolDown);
        CheckCoolDown(chickenImage, skill.chicken.coolDown);
        CheckCoolDown(amuletImage, 0.5f);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStat.GetMaxHealthValue();
        slider.value = playerStat.currentHealth;
    }

    private void SetCoolDown(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCoolDown(Image _image, float _coolDown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _coolDown * Time.deltaTime;
    }
}
