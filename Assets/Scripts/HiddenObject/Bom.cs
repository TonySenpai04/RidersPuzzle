using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : HiddenObject
{
    public override void ActiveSkill()
    {
        PlayerController.instance.hitPoint.TakeDamage(3);
        PlayerController.instance.movementController.MoveForward(2);
        DestroyObject();


    }
}
