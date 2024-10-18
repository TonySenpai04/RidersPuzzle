using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockgate : HiddenObject
{
 
    public override void ActiveSkill()
    {

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
    private void FixedUpdate()
    {
        if (LevelManager.instance.GetCurrentLevelKey() > 0)
        {
            GetComponentInParent<BoxCollider2D>().enabled = true;
        }
    }
}
