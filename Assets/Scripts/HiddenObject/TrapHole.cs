using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHole : HiddenObject
{
    public override void ActiveSkill()
    {
        PlayerController.instance.hitPoint.TakeDamage(3);
        PlayerController.instance.movementController.numberOfMoves.ReduceeMove(3);
        DestroyObject();
    }
}
