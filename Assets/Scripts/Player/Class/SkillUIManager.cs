using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private Image skillImage; 
    [SerializeField] private Sprite activeSkillSprite; 
    [SerializeField] private Sprite inactiveSkillSprite;

    private void FixedUpdate()
    {
        UpdateSkillImage();
    }
    public void UpdateSkillImage()
    {
      ISkill currentSkill=  SkillManager.instance.GetCurrentSkill();
        if (currentSkill.GetNumberOfSkill() > 0 )
        {
            skillImage.sprite = activeSkillSprite;
        }
        else
        {
            skillImage.sprite = inactiveSkillSprite;
        }
    }

}
