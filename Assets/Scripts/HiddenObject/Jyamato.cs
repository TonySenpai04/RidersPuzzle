using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jyamato : HiddenObject
{
    public override void ActiveSkill()
    {
        PlaySFX();
        PlayerController.instance.hitPoint.TakeDamage(3);
        DestroyObject();
    }
}
