using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeatsSkill : ISkill
{
    private GridController gridController;

    public GeatsSkill(GridController gridController)
    {
        this.gridController = gridController;
  
    }

    public void ActivateSkill()
    {
        int characterRow = PlayerController.instance.movementController.GetPos().Item1;
        int characterCol = PlayerController.instance.movementController.GetPos().Item2;

        // Xoá tất cả object trong phạm vi 3x3
        for (int row = characterRow - 1; row <= characterRow + 1; row++)
        {
            for (int col = characterCol - 1; col <= characterCol + 1; col++)
            {
                if (row >= 0 && row < gridController.rows && col >= 0 && col < gridController.cols)
                {
                    GameObject cell = LevelManager.instance.CheckForHiddenObject(row, col);
                    if (cell != null) 
                    {
                        HiddenObject hiddenObjComponent = cell.GetComponent<HiddenObject>();
                        if (hiddenObjComponent != null)
                        {
                            hiddenObjComponent.DestroyObject(); 
                        }
                    }
                }
            }
        }
        PlayerController.instance.movementController.numberOfMoves.IncreaseMove(1);
    }

}
