using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jyamato : HiddenObject
{
    public override void ActiveSkill()
    {
       PlayerController.instance.movementController.UndoLastMove(1);
       PlayerController.instance.hitPoint.TakeDamage(1);
    }
}
