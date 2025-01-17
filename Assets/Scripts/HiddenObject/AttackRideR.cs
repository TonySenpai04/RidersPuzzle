using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRideR : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying) // Kiểm tra nếu đối tượng đang biến mất
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        var currentPos = PlayerController.instance.movementController.GetPos();
        int currentRow = currentPos.Item1;
        List<GameObject> hiddenObjectsInRow =  LevelManager.instance.GetHiddenObjectsInRow(currentRow);

        foreach (var hiddenObject in hiddenObjectsInRow)
        {
            if (hiddenObject != null)
            {
                HiddenObject hiddenObj = hiddenObject.GetComponent<HiddenObject>();
                if (hiddenObj != null && hiddenObj.type == ObjectType.Obstacle)
                {
                    hiddenObj.GetComponentInParent<BoxCollider2D>().enabled = true;
                    Destroy(hiddenObject);
                }
            }
        }
        DestroyObject();
    }

}
