using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : HiddenObject
{
    public override void ActiveSkill()
    {
   
        Tuple<int, int> lastMove = PlayerController.instance.movementController.GetLastMove();
        if (lastMove == null)
        {
            return; 
        }

        Vector2Int lastPosition = new Vector2Int(lastMove.Item1, lastMove.Item2);
        Vector2Int currentPosition = new Vector2Int(PlayerController.instance.movementController.GetPos().Item1,
            PlayerController.instance.movementController.GetPos().Item2);

        foreach (var entry in LevelManager.instance.hiddenObjectInstances)
        {
            Teleport hiddenObject = entry.Value.GetComponent<Teleport>();
            if (hiddenObject != null && hiddenObject != this)
            {
                Vector2Int teleportPosition = new Vector2Int(entry.Key.x, entry.Key.y);


                int deltaX = Mathf.Abs(currentPosition.x - lastPosition.x);
                int deltaY = Mathf.Abs(currentPosition.y - lastPosition.y);
                Debug.Log(deltaX + " " + deltaY);

                if ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))
                {

                    PlayerController.instance.movementController.MoveToBlock(teleportPosition.x,teleportPosition.y);
                    return;
                       
                    
                }
            }
        }
    }
}
