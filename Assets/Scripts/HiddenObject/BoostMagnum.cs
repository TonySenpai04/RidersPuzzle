using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMagnum : HiddenObject
{
    public override void ActiveSkill()
    {

        PlayerController.instance.movementController.numberOfMoves.IncreaseMove(2);
        DestroyObject();

    }
}
