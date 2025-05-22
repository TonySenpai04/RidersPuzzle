using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilHorn : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        int targetRow = 1;
        for (int col = 0; col < LevelManager.instance.GetGrid().cols; col++)
        {
            GameObject obj = LevelManager.instance.CheckForHiddenObject(targetRow, col);
            if (obj != null)
            {
                obj.GetComponent<HiddenObject>().DestroyObject();
            }

          
        }
        PlayerController.instance.movementController.MoveToBlock(1, 4);
        DestroyObject();
    }
}
