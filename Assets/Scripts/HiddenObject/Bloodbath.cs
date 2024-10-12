using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodbath : HiddenObject
{
    public override void ActiveSkill()
    {
        PlayerController.instance.hitPoint.TakeDamage(2);
        PlayerController.instance.movementController.numberOfMoves.IncreaseMove(4);
        DestroyObject();
    }
}
