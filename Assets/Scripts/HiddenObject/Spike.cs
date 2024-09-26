using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : HiddenObject
{
    public override void ActiveSkill()
    {

        PlayerController.instance.hitPoint.TakeDamage(1);
    }
}
