using System;
using System.Collections.Generic;
using UnityEngine;

public class DarkIncense: HiddenObject
{
    public GameObject jyamatoPrefab;

    public override void ActiveSkill()
    {
        var currentPos = PlayerController.instance.movementController.GetPos();
        int currentRow = currentPos.Item1;
        int currentCol = currentPos.Item2;

        Vector2Int[] spawnPositions = new Vector2Int[]
       {
        new Vector2Int(  currentRow+1,currentCol),
        new Vector2Int(currentRow -1, currentCol),
        new Vector2Int(currentRow , currentCol-1),
        new Vector2Int(currentRow , currentCol+1)
 
        };
        foreach (Vector2Int spawnPos in spawnPositions)
        {
            GameObject cell = LevelManager.instance.gridController.grid[(int)spawnPos.x,(int) spawnPos.y];

            GameObject hiddenObject = Instantiate(jyamatoPrefab, cell.transform.position, Quaternion.identity);
            hiddenObject.SetActive(true);
            LevelManager.instance.AddHiddenObjectToCurrentLevel(spawnPos.x,spawnPos.y, hiddenObject);
            hiddenObject.transform.SetParent(cell.transform);
            
        }
 
        DestroyObject();


    }
}
