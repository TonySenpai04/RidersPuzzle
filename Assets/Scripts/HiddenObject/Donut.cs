using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donut : HiddenObject
{
    public override void ActiveSkill()
    {

        PlayerController.instance.hitPoint.Heal(1);
        DestroyObject();
    }
}
