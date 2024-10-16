using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRideC : HiddenObject
{
    public override void ActiveSkill()
    {
        var currentPos = PlayerController.instance.movementController.GetPos();
        int currentCol = currentPos.Item2;
        List<GameObject> hiddenObjectsInRow = LevelManager.instance.GetHiddenObjectsInColumn(currentCol);

        foreach (var hiddenObject in hiddenObjectsInRow)
        {
            HiddenObject hiddenObj = hiddenObject.GetComponent<HiddenObject>();
            if (hiddenObj != null && hiddenObj.type == ObjectType.Obstacle)
            {
                Destroy(hiddenObject);
            }
        }
        DestroyObject();
    }
}
