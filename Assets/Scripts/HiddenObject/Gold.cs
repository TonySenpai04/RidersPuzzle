using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        GoldManager.instance.AddGold(3);
        DestroyObject();
    }
}
