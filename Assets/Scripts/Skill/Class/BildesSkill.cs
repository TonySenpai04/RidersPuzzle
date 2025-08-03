using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BildesSkill : BaseSkill
{

    public BildesSkill(GridController gridController, int skillAmount, int id)
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

            List<GameObject> hiddenObjects = new List<GameObject>();
        for (int row = 0; row < gridController.rows; row++)
        {
            for (int col = 0; col < gridController.cols; col++)
            {
                GameObject obj = LevelManager.instance.CheckForHiddenObject(row, col);
                if (obj != null)
                {
                    hiddenObjects.Add(obj);
                }
            }
        }

        // 2. Trộn ngẫu nhiên danh sách
        for (int i = 0; i < hiddenObjects.Count; i++)
        {
            GameObject temp = hiddenObjects[i];
            int randomIndex = Random.Range(i, hiddenObjects.Count);
            hiddenObjects[i] = hiddenObjects[randomIndex];
            hiddenObjects[randomIndex] = temp;
        }

        // 3. Xóa ngẫu nhiên `destroyCount` object đầu tiên
        for (int i = 0; i < destroyCount && i < hiddenObjects.Count; i++)
        {
            hiddenObjects[i].GetComponent<HiddenObject>().DestroyObject();
        }



            if (healAmount > 0)
            {
                PlayerController.instance.hitPoint.Heal(healAmount);
            }

            skillAmount--; 
        }
    }

}
