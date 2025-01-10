using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : HiddenObject
{
    public override void ActiveSkill()
    {
        PlaySFX();
        LevelManager.instance.IncreaseLevelKey();
        DestroyObject();
    }
}
