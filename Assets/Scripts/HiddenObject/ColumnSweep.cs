using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSweep : HiddenObject
{
    public override void ActiveSkill()
    {
        PlaySFX();
        ClearColumnAndHeal();
        DestroyObject();
    }

    public void ClearColumnAndHeal()
    {
        int currentCol = PlayerController.instance.movementController.GetPos().Item2;

        List<GameObject> objectsInSameColumn = LevelManager.instance.GetHiddenObjectsInColumn(currentCol);
        foreach (GameObject obj in objectsInSameColumn)
        {
            if (obj != null)
            {
                obj.GetComponentInParent<BoxCollider2D>().enabled = true;
               Destroy(obj);
            }
        }
        PlayerController.instance.hitPoint.Heal(1);
        Debug.Log("Xoá bỏ tất cả bùa lợi/chướng ngại vật trong cột " + currentCol + " và hồi phục 1 HP.");
    }

}
