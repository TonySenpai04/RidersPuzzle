using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : HiddenObject
{
    public override void Start()
    {
       if(isPandora)
        {
            PlayerController.instance.movementController.UndoLastMove(1);
            isPandora = false;
        }
    }


    //public override void ActiveSkill()
    //{
    //    if (isDestroying)
    //    {
    //        Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
    //        return;
    //    }
    //    Vector2Int direction = PlayerController.instance.movementController.GetCurrentDirection();

    //    var currentPos = PlayerController.instance.movementController.GetPos();
    //    int currentRow = currentPos.Item1 + direction.x;
    //    int currentCol = currentPos.Item2 + direction.y;

    //    int mapRows = LevelManager.instance.GetGrid().rows; 
    //    int mapCols = LevelManager.instance.GetGrid().cols;
    //    Vector2 spawnPos = new Vector2(currentRow, currentCol);
    //    if (LevelManager.instance.CheckForHiddenObject(currentRow, currentCol) == null)
    //    {
    //        if (currentRow >= 0 && currentRow < mapRows && currentCol >= 0
    //                && currentCol < mapCols && spawnPos != LevelManager.instance.GetCurrentLevelData().endPos)
    //        {
    //            GameObject cell = LevelManager.instance.GetGrid().grid[(int)currentRow, currentCol];
    //            Vector2Int positionKey = LevelManager.instance.hiddenObjectInstances.FirstOrDefault(
    //     kvp => kvp.Value == this.gameObject
    // ).Key;
    //            //  GameObject hiddenObject = Instantiate(this.gameObject, cell.transform.position, Quaternion.identity);
    //            transform.position=cell.transform.position; 
    //          LevelManager.instance.hiddenObjectInstances[positionKey] = null;
    //            LevelManager.instance.AddHiddenObjectToCurrentLevel(currentRow, currentCol, this.gameObject);
    //            transform.SetParent(cell.transform);

    //        }

    //    }
    //    else
    //    {
    //        PlayerController.instance.movementController.UndoLastMove(1);
    //    }
    //    //PlaySFX();
    //    //GetComponentInParent<BoxCollider2D>().enabled = false;
    //    //PlayerController.instance.movementController.UndoLastMove(1);


    //}
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        if (isPandora)
        {
            return;
        }

        PlaySFX();
        Vector2Int direction = PlayerController.instance.movementController.GetCurrentDirection();
        var currentPos = PlayerController.instance.movementController.GetPos();
        int currentRow = currentPos.Item1 + direction.x;
        int currentCol = currentPos.Item2 + direction.y;

        int mapRows = LevelManager.instance.GetGrid().rows;
        int mapCols = LevelManager.instance.GetGrid().cols;

        Vector2 spawnPos = new Vector2(currentRow, currentCol);

        if (currentRow >= 0 && currentRow < mapRows && currentCol >= 0 && currentCol < mapCols
            && spawnPos != LevelManager.instance.GetCurrentLevelData().endPos)
        {
            GameObject cell = LevelManager.instance.GetGrid().grid[currentRow, currentCol];
            var hiddenObject = LevelManager.instance.CheckForHiddenObject(currentRow, currentCol);

            if (hiddenObject == null)
            {
                GameObject HI = Instantiate(this.gameObject, cell.transform.position, Quaternion.identity);
                LevelManager.instance.AddHiddenObjectToCurrentLevel(currentRow, currentCol, HI);
                HI.transform.SetParent(cell.transform);
                Destroy(this.gameObject);

            }
            else
            {
                PlayerController.instance.movementController.UndoLastMove(1);
            }
        }
        else
        {
            PlayerController.instance.movementController.UndoLastMove(1);
        }

    }

    public override void DestroyObject()
    {
        //GetComponentInParent<BoxCollider2D>().enabled = true;
        base.DestroyObject();
    }

}
