using UnityEngine;

internal class NigoutoSkill : BaseSkill
{


    public NigoutoSkill(GridController gridController, int skillAmount,int id)
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
            int currentRow = PlayerController.instance.movementController.GetPos().Item1;
            int currentCol = PlayerController.instance.movementController.GetPos().Item2;

            Vector2Int[] positions = new Vector2Int[]
            {
                new Vector2Int(currentRow, currentCol),
            new Vector2Int(currentRow + 1, currentCol),
            new Vector2Int(currentRow - 1, currentCol),
            new Vector2Int(currentRow, currentCol - 1),
            new Vector2Int(currentRow, currentCol + 1)
            };


            foreach (Vector2Int pos in positions)
            {
                if (pos.x >= 0 && pos.x < gridController.rows &&
                    pos.y >= 0 && pos.y < gridController.cols)
                {
                    GameObject cell = LevelManager.instance.CheckForHiddenObject(pos.x, pos.y);
                    if (cell != null)
                    {
                        HiddenObject hiddenObjComponent = cell.GetComponent<HiddenObject>();
                        if (hiddenObjComponent != null)
                        {
                            hiddenObjComponent.gameObject.SetActive(true);
                            hiddenObjComponent.DestroyObject();
                            PlayerController.instance.hitPoint.Heal(1);
                        }
                    }
                }
            }
            skillAmount--;
        }
    }

 
}