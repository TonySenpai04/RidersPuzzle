using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : HiddenObject
{
    public GameObject backTeleport;
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }

        PlaySFX();
        Tuple<int, int> lastMove = PlayerController.instance.movementController.GetLastMove();
        if (lastMove == null)
        {
            return;
        }
        Vector2Int lastPosition = new Vector2Int(lastMove.Item1, lastMove.Item2);
        Vector2Int currentPosition = new Vector2Int(PlayerController.instance.movementController.GetPos().Item1,
            PlayerController.instance.movementController.GetPos().Item2);
        if (backTeleport != null)
        {
            foreach (var entry in LevelManager.instance.hiddenObjectInstances)
            {
                if (entry.Value == backTeleport)
                {
                    Vector2Int teleportPos = new Vector2Int(entry.Key.x,entry.Key.y);
                    int deltaX = Mathf.Abs(currentPosition.x - lastPosition.x);
                    int deltaY = Mathf.Abs(currentPosition.y - lastPosition.y);
                    if ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1) || (deltaX == 2 && deltaY == 0) || (deltaX == 0 && deltaY == 2))
                    {

                        PlayerController.instance.movementController.MoveToBlock(teleportPos.x, teleportPos.y);

                    }

                }
            }
           

        }
        else
        {

            List<Teleport> otherTeleports = new List<Teleport>();
            foreach (var entry in LevelManager.instance.hiddenObjectInstances)
            {
                if (entry.Value != null)
                {
                    Teleport teleportObj = entry.Value.GetComponent<Teleport>();
                    if (teleportObj != null && teleportObj != this)
                    {
                        otherTeleports.Add(teleportObj);
                    }
                }
            }

            if (otherTeleports.Count == 0)
            {
                Debug.Log("Không có teleport nào khác để dịch chuyển.");
                return;
            }

            Teleport targetTeleport = otherTeleports[UnityEngine.Random.Range(0, otherTeleports.Count)];
            targetTeleport.backTeleport = this.gameObject;
        Debug.Log(targetTeleport.backTeleport);
            foreach (var entry in LevelManager.instance.hiddenObjectInstances)
            {
                if (entry.Value == targetTeleport.gameObject)
                {
                    Vector2Int teleportPos = new Vector2Int(entry.Key.x, entry.Key.y);
                    int deltaX = Mathf.Abs(currentPosition.x - lastPosition.x);
                    int deltaY = Mathf.Abs(currentPosition.y - lastPosition.y);
                    if ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1) || (deltaX == 2 && deltaY == 0) || (deltaX == 0 && deltaY == 2))
                    {

                        PlayerController.instance.movementController.MoveToBlock(teleportPos.x, teleportPos.y);
                        return;
                    }
                    break;
                }
            }
        }

    }


}
