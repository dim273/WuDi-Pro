using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        entity = GetComponentInParent<Entity>();
        myStats = GetComponentInParent<CharacterStats>();

        UpdateHealthUI();

        entity.onFilpped += FilpUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void Update()
    {
        
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();

        slider.value = myStats.currentHealth;
    }

    private void FilpUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
    private void OnDisable()
    {
        entity.onFilpped -= FilpUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }

}
