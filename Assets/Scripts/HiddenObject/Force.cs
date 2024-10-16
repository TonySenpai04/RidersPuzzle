using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : HiddenObject
{
    public override void ActiveSkill()
    {
        PlayerController.instance.movementController.MoveForward(2);
        PlayerController.instance.movementController.numberOfMoves.ReduceeMove(4);
        DestroyObject();
    }
}
