using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillSelection
{
    void CreateSkillButtons(List<int> heroIds);
    void SelectSkill(int index);
    ISkill GetSelectedSkill();
    void ClearSelection();
}
