using UnityEngine;

internal class GavvSkill : ISkill
{
    private int maxSkillAmount;
    private int skillAmount;
    private GridController gridController;

    public GavvSkill(GridController gridController, int skillAmount)
    {
        this.gridController = gridController;
        this.maxSkillAmount = skillAmount;
        this.skillAmount = skillAmount;


    }

    public void ActivateSkill()
    {
        if (skillAmount > 0)
        {
            int currentRow = PlayerController.instance.movementController.GetPos().Item1;
            int currentCol = PlayerController.instance.movementController.GetPos().Item2;

            Vector2Int[] positions = new Vector2Int[]
            {
            new Vector2Int(currentRow + 1, currentCol),
            new Vector2Int(currentRow - 1, currentCol),
            new Vector2Int(currentRow, currentCol - 1),
            new Vector2Int(currentRow, currentCol + 1)
            };

            // Duyệt qua các vị trí và xử lý
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
                            hiddenObjComponent.DestroyObject();
                            PlayerController.instance.hitPoint.Heal(1);
                        }
                    }
                }
            }
            skillAmount--;
        }
    }

    public int GetNumberOfSkill()
    {
        return skillAmount;
    }

    public void IncreaseUses(int amount)
    {
        skillAmount += amount;
        if (skillAmount > maxSkillAmount)
        {
            skillAmount = maxSkillAmount;
        }


    }

    public void SetNumberOfSkill(int amount)
    {
        this.skillAmount = amount;
    }
}