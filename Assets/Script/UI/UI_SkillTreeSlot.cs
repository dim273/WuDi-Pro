using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour
{
    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;

    [SerializeField] private Image skillImage;

    private void Start()
    {
        skillImage = GetComponent<Image>();

        skillImage.color = Color.red;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        for(int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if(shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }
        for(int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return; 
            }
        }
        unlocked = true;
        skillImage.color = Color.green;
    }

}
