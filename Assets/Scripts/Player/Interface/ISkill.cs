using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill 
{
    void ActivateSkill();
    void SetNumberOfSkill(int amount);
    void IncreaseUses(int amount);
}
