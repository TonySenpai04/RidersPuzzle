using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorWish : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        PlayerController.instance.hitPoint.Heal(1);
        PlayerController.instance.movementController.numberOfMoves.IncreaseMove(1);
        DestroyObject();
    }
}
