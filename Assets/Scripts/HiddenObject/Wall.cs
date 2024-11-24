using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : HiddenObject
{
    public override void ActiveSkill()
    {
        GetComponentInParent<BoxCollider2D>().enabled = false;
        PlayerController.instance.movementController.UndoLastMove(1);


    }
    public override void DestroyObject()
    {
        GetComponentInParent<BoxCollider2D>().enabled = true;
        base.DestroyObject();
    }

}
