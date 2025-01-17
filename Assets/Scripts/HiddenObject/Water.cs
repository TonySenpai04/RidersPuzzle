using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : HiddenObject
{
    public override void Start()
    {

        GetComponentInParent<BoxCollider2D>().enabled = false;
    }
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        GetComponentInParent<BoxCollider2D>().enabled = false;
        PlayerController.instance.movementController.UndoLastMove(1);
    }
    public override void DestroyObject()
    {
        GetComponentInParent<BoxCollider2D>().enabled = true;
        base.DestroyObject();
    }
}

