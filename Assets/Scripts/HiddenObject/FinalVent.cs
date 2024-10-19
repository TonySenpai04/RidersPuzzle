using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalVent : HiddenObject
{
    public override void ActiveSkill()
    {
        PlayerController.instance.movementController.immortal.ActivateImmortalEffect();
        DestroyObject();


    }
}
