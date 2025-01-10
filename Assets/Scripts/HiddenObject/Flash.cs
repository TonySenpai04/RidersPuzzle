using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : HiddenObject
{
    public override void ActiveSkill()
    {
        PlaySFX();
        PlayerController.instance.movementController.MoveForward(3);
        DestroyObject();

    }
}