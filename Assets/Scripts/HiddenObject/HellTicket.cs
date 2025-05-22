using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class HellTicket : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }

        PlaySFX();
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        for (int row = 0; row < LevelManager.instance.GetGrid().rows; row++)
        {
            for (int col = 0; col < LevelManager.instance.GetGrid().cols; col++)
            {
                GameObject obj = LevelManager.instance.CheckForHiddenObject(row, col);
                Vector2Int currentPos =new Vector2Int( PlayerController.instance.movementController.GetPos().Item1,
                    PlayerController.instance.movementController.GetPos().Item2);

                // Không tính ô hiện tại & chỉ chọn ô không có object
                if ((row != currentPos.x || col != currentPos.y) && obj == null)
                {
                    emptyPositions.Add(new Vector2Int(row, col));
                }
            }
        }
        if (emptyPositions.Count > 0)
        {
            Vector2Int randomPos = emptyPositions[Random.Range(0, emptyPositions.Count)];
            PlayerController.instance.movementController.MoveToBlock(randomPos.x, randomPos.y); // Hàm cần tự bạn hoặc bạn đã có

            PlayerController.instance.hitPoint.TakeDamage(3);

            Debug.Log($"Teleported to {randomPos.x},{randomPos.y} and lost 3 HP.");
        }
        DestroyObject();
    }
}