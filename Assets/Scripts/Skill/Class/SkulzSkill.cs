using UnityEngine;

internal class SkulzSkill : BaseSkill
{
   
    public SkulzSkill(GridController gridController, int skillAmount, int id)
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
            Vector2Int direction = PlayerController.instance.movementController.GetCurrentDirection();
            var currentPos = PlayerController.instance.movementController.GetPos();
            int currentRow = currentPos.Item1;
            int currentCol = currentPos.Item2;
            for (int i = 1; i <= 2; i++)
            {
                int targetRow = currentRow + direction.x * i;
                int targetCol = currentCol + direction.y * i;

                if (targetRow >= 0 && targetRow < gridController.rows &&
                    targetCol >= 0 && targetCol < gridController.cols)
                {
                    GameObject cell = LevelManager.instance.CheckForHiddenObject(targetRow, targetCol);
                    if (cell != null)
                    {
                        HiddenObject hiddenObjComponent = cell.GetComponent<HiddenObject>();
                        if (hiddenObjComponent != null)
                        {
                            hiddenObjComponent.gameObject.SetActive(true);
                            hiddenObjComponent.DestroyObject();
                        }
                    }
                }
                SoundManager.instance.PlayHeroSFX(id);
                PlayerController.instance.hitPoint.Heal((int)PlayerController.instance.hitPoint.GetMaxHealth());
                skillAmount--;
            }
        }
    }

 
}