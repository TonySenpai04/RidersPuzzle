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
        if (skillAmount <= 0) return;

        Vector2Int direction = PlayerController.instance.movementController.GetCurrentDirection();
        var currentPos = PlayerController.instance.movementController.GetPos();
        int currentRow = currentPos.Item1;
        int currentCol = currentPos.Item2;

        bool isTop = currentRow == 0;
        bool isBottom = currentRow == gridController.rows - 1;
        bool isLeft = currentCol == 0;
        bool isRight = currentCol == gridController.cols - 1;

        bool isInCorner = (isTop && isLeft) || (isTop && isRight) || (isBottom && isLeft) || (isBottom && isRight);

        if (isInCorner)
        {
            // Xóa 1 ô cùng hàng gần nhân vật
            if (currentCol == 0)
                RemoveHiddenObject(currentRow, currentCol + 1); // ô bên phải
            else
                RemoveHiddenObject(currentRow, currentCol - 1); // ô bên trái

            // Xóa 1 ô cùng cột gần nhân vật
            if (currentRow == 0)
                RemoveHiddenObject(currentRow + 1, currentCol); // ô bên dưới
            else
                RemoveHiddenObject(currentRow - 1, currentCol); // ô bên trên
        }
        else
        {
            // Xử lý như cũ: xóa 2 ô phía trước theo hướng di chuyển
            for (int i = 1; i <= 2; i++)
            {
                int targetRow = currentRow + direction.x * i;
                int targetCol = currentCol + direction.y * i;

                if (targetRow >= 0 && targetRow < gridController.rows &&
                    targetCol >= 0 && targetCol < gridController.cols)
                {
                    RemoveHiddenObject(targetRow, targetCol);
                }
            }
        }

        SoundManager.instance.PlayHeroSFX(id);
        PlayerController.instance.hitPoint.Heal((int)PlayerController.instance.hitPoint.GetMaxHealth());
        skillAmount--;
    }
    private void RemoveHiddenObject(int row, int col)
    {
        GameObject cell = LevelManager.instance.CheckForHiddenObject(row, col);
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

}