using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItsuiSkill : BaseSkill
{
    

    public ItsuiSkill(GridController gridController,int skillAmount, int id)
    {
        this.gridController = gridController;
        this.maxSkillAmount = skillAmount;
        this.skillAmount = skillAmount;

        this.id = id;
    }

    public override void ActivateSkill()
    {
        if (skillAmount > 0)
        {
            SoundManager.instance.PlayHeroSFX(id);
            int characterRow = PlayerController.instance.movementController.GetPos().Item1;
            int characterCol = PlayerController.instance.movementController.GetPos().Item2;

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
            skillAmount--;
        }
    }

  
}
