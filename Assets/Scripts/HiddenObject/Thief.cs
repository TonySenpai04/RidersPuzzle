using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : HiddenObject
{
    public override void ActiveSkill()
    {
        PlaySFX();
        PlayerController.instance.movementController.numberOfMoves.ReduceeMove(4);
        DestroyObject();
    }
}
