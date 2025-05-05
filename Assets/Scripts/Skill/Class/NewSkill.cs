using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSkill : BaseSkill
{

    public NewSkill(GridController gridController, int skillAmount, int id)
    {
        this.gridController = gridController;
        this.maxSkillAmount = skillAmount;
        this.skillAmount = skillAmount;

        this.id = id;
    }
    public override void ActivateSkill()
    {
        if (skillAmount > 0)
        {
            SoundManager.instance.PlayHeroSFX(id);

            float currentHp = PlayerController.instance.hitPoint.GetCurrentHealth();
            float maxHp = PlayerController.instance.hitPoint.GetMaxHealth();
            int lostHp = Mathf.FloorToInt(maxHp - currentHp);

            int objectCount = gridController.GetCurrentObjectsInMap(); 

            int destroyCount = Mathf.Min(lostHp, objectCount);
            int healAmount = lostHp - destroyCount; 

            int destroyed = 0;
            for (int row = 0; row < gridController.rows; row++)
            {
                for (int col = 0; col < gridController.cols; col++)
                {
                    if (destroyed >= destroyCount)
                        break;

                    GameObject obj = LevelManager.instance.CheckForHiddenObject(row, col);
                    if (obj != null)
                    {

                        obj.GetComponent<HiddenObject>().DestroyObject();
                        destroyed++;
                    }
                }
                if (destroyed >= destroyCount)
                    break;
            }


            if (healAmount > 0)
            {
                PlayerController.instance.hitPoint.Heal(healAmount);
            }

            skillAmount--; 
        }
    }

}
