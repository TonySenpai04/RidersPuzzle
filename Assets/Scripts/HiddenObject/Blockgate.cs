using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blockgate : HiddenObject
{
    public override void Start()
    {
        GetComponentInParent<BoxCollider2D>().enabled = false;
    }
    public override void ActiveSkill()
    {
        PlaySFX();
        if (LevelManager.instance.GetCurrentLevelKey() > 0)
        {
            
            LevelManager.instance.DecreaseLevelKey();
            PlayerController.instance.hitPoint.Heal(1);
            DestroyObject();
        }
        else
        {
          
            GetComponentInParent<BoxCollider2D>().enabled = false;
            PlayerController.instance.movementController.UndoLastMove(1);
        }


    }
    public override void DestroyObject()
    {
        GetComponentInParent<BoxCollider2D>().enabled = true;
        base.DestroyObject();
    }
    private void FixedUpdate()
    {
        if (LevelManager.instance.GetCurrentLevelKey() > 0)
        {
            GetComponentInParent<BoxCollider2D>().enabled = true;
        }
    }
}
