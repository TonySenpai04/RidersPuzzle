using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowSweep : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        ClearRowAndHeal();
        DestroyObject();
    }
    public void ClearRowAndHeal()
    {
        int currentRow = PlayerController.instance.movementController.GetPos().Item1;

        List<GameObject> objectsInSameRow = LevelManager.instance.GetHiddenObjectsInRow(currentRow);

        foreach (GameObject obj in objectsInSameRow)
        {
            if (obj != null)
            {
                obj.GetComponentInParent<BoxCollider2D>().enabled = true;
                Destroy(obj);
            }
        }

        PlayerController.instance.hitPoint.Heal(1);

        Debug.Log("Xoá bỏ tất cả bùa lợi/chướng ngại vật trong hàng " + currentRow + " và hồi phục 1 HP.");
    }

}
