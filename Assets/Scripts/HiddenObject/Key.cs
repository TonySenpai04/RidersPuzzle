using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : HiddenObject
{
    public override void ActiveSkill()
    {
        LevelManager.instance.IncreaseLevelKey();
        DestroyObject();
    }
}
