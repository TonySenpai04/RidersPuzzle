using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : HiddenObject
{
    public override void ActiveSkill()
    {
        PlaySFX();
        GoldManager.instance.AddGold(3);
        DestroyObject();
    }
}
