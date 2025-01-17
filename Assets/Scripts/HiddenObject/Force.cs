using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        PlayerController.instance.movementController.MoveForward(2);
        PlayerController.instance.movementController.numberOfMoves.ReduceeMove(4);
        DestroyObject();
    }
}
