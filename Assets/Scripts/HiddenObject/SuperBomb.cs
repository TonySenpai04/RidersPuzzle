using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBomb : HiddenObject
{
    public override void ActiveSkill()
    {
        PlaySFX();
        PlayerController.instance.hitPoint.TakeDamage(10);
        DestroyObject();
    }
}
