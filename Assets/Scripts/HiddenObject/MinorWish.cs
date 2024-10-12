using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorWish : HiddenObject
{
    public override void ActiveSkill()
    {
        PlayerController.instance.hitPoint.Heal(1);
        PlayerController.instance.movementController.numberOfMoves.IncreaseMove(1);
        DestroyObject();
    }
}
