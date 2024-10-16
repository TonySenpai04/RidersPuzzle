using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRideR : HiddenObject
{
    public override void ActiveSkill()
    {
        var currentPos = PlayerController.instance.movementController.GetPos();
        int currentRow = currentPos.Item1;
        List<GameObject> hiddenObjectsInRow =  LevelManager.instance.GetHiddenObjectsInRow(currentRow);

        foreach (var hiddenObject in hiddenObjectsInRow)
        {
            HiddenObject hiddenObj = hiddenObject.GetComponent<HiddenObject>(); 
            if (hiddenObj != null && hiddenObj.type==ObjectType.Obstacle)
            {
                Destroy(hiddenObject); 
            }
        }
        DestroyObject();
    }

}
