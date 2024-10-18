using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhance : HiddenObject
{
    public override void ActiveSkill()
    {
        SkillManager.instance.IncreaseSkillUsesForCurrentHero(1);
        DestroyObject();
    }
}
