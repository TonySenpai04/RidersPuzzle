using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : HiddenObject
{
    public override void ActiveSkill()
    {
        GetComponentInParent<BoxCollider2D>().enabled = false;
        PlayerController.instance.movementController.UndoLastMove(1);
    }
}

