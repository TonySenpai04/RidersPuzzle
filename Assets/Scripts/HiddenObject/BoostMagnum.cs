using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMagnum : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying) // Kiểm tra nếu đối tượng đang biến mất
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        PlayerController.instance.movementController.numberOfMoves.IncreaseMove(2);
        DestroyObject();


    }
}
