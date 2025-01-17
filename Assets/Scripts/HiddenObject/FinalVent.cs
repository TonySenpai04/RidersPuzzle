using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalVent : HiddenObject
{
    public override void ActiveSkill()
    {
        if (isDestroying)
        {
            Debug.Log("Không thể kích hoạt skill vì đối tượng đang biến mất.");
            return;
        }
        PlaySFX();
        PlayerController.instance.movementController.immortal.ActivateImmortalEffect();
        DestroyObject();


    }
}
