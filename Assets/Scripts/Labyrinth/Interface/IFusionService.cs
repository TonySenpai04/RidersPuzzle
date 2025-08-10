using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFusionService 
{
    DataHero FuseHeroes(List<int> heroIds, ISkill skill);
}
